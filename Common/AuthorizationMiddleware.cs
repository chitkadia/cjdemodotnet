using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FileUpload.Common
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        private IConfiguration config;
        public AuthorizationMiddleware(IConfiguration _config, RequestDelegate _next)
        {
            next = _next;
            config = _config;
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

                    var mySecret = config["Jwt:Key"];
                    var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

                    var myIssuer = config["Jwt:Issuer"];
                    var myAudience = config["Jwt:Issuer"];

                    var tokenHandler = new JwtSecurityTokenHandler();

                    if (encryptedAccessToken.Contains("Bearer"))
                    {
                        encryptedAccessToken = encryptedAccessToken.Replace("Bearer ", "");
                    }

                    string accessToken = encryptedAccessToken;

                    var principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = myIssuer,
                        ValidAudience = myAudience,
                        IssuerSigningKey = mySecurityKey,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    }, out SecurityToken validatedToken);



                    if (principal.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
                    {
                        context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                                        {
                                            new Claim(ClaimTypes.Name, principal.Identity.Name)
                                        }, "BasicAuthentication"));
                        context.User.AddIdentity((ClaimsIdentity)principal.Identity);

                        await next.Invoke(context);
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
                        StatusCode = StatusCodes.TokenExpired,
                        message = "Token expired",
                    })));
                }
                else if (!string.IsNullOrEmpty(ex.Message) && (ex.Message.Contains("JWT") || ex.Message.Contains("jwt") || ex.Message.Contains("Jwt") || ex.Message.Contains("validation failed")))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject((new
                    {
                        StatusCode = StatusCodes.InvalidToken,
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
                        StatusCode = StatusCodes.ServerError,
                        message = ex.Message.ToString(),
                    })));
                }

            }
        }
    }
}
