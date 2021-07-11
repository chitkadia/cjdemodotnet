using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FileUpload.Repositories;
using FileUpload.Entities;

namespace FileUpload.Common
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        private IConfiguration config;
        IUserRepository userRepository { get; set; }
        public AuthorizationMiddleware(IConfiguration _config, RequestDelegate _next, IUserRepository _userRepository)
        {
            next = _next;
            config = _config;
            userRepository = _userRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string encryptedAccessToken = string.IsNullOrEmpty(context.Request.Headers["Authorization"]) ? "" : Convert.ToString(context.Request.Headers["Authorization"]);
                if (string.IsNullOrEmpty(encryptedAccessToken))
                {
                    await next.Invoke(context);
                }
                else
                {
                    if (encryptedAccessToken.Contains("Bearer"))
                    {
                        encryptedAccessToken = encryptedAccessToken.Replace("Bearer ", "");
                    }

                    string accessToken = encryptedAccessToken;
                    var isValidToken = await userRepository.checkAuthTokenValidOrNot(accessToken);

                    if (isValidToken) {
                        await next.Invoke(context);
                    } else {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject((new
                        {
                            StatusCode = 401,
                            message = "Token expired",
                        })));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SecurityTokenExpiredException || (!string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("token is expired")))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject((new
                    {
                        StatusCode = 401,
                        message = "Token expired",
                    })));
                }
                else if (!string.IsNullOrEmpty(ex.Message) && (ex.Message.Contains("JWT") || ex.Message.Contains("jwt") || ex.Message.Contains("Jwt") || ex.Message.Contains("validation failed")))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject((new
                    {
                        StatusCode = 401,
                        message = "Invalid token",
                    })));
                }
                else
                {
                    //Do not comment this, important for logging error to file /Logs/{CurrentDate}
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject((new
                    {
                        StatusCode = 500,
                        message = ex.Message.ToString(),
                    })));
                }

            }
        }
    }
}
