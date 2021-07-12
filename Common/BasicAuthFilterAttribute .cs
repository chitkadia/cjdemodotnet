using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Common
{
    public class BasicAuthFilterAttribute : ActionFilterAttribute
    {
        private StringValues authorizationToken;
        

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var authHeader = actionContext.HttpContext.Request.Headers.TryGetValue("Token", out authorizationToken);


            actionContext.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject((new
            {
                StatusCode = "802",
                message = "Invalid Token.",
                data = string.Empty
            })));
        }
    }
}
