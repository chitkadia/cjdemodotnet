using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using FileUpload.Entities;

namespace FileUpload.Common
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            #region  // Start API Method call time 
            try
            {
                var controllerName = context.RouteData.Values["controller"];
                var actionName = context.RouteData.Values["action"];
                var method = context.HttpContext.Request.Method;
                var identity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null && identity.IsAuthenticated)
                {
                    var user_id = identity.Claims.Where(p => p.Type == "user_id").FirstOrDefault()?.Value;
                    var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (token != null)
                    {
                        APIMethodCall ApiCall = new APIMethodCall();
                        ApiCall.APIName = controllerName + "/" + actionName;
                        ApiCall.APIMethod = method;
                        ApiCall.APIToken = token;
                        ApiCall.user_id = user_id;
                    }
                }
            }
            catch (Exception ex)
            {}
            #endregion
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            #region  // Logs API Method call end time
            try
            {
                //var apiLogID = context.HttpContext.Items[Constants.APILogID];
                //if (apiLogID != null)
                //{
                //    var commonRepository = context.HttpContext.RequestServices.GetService<ICommonRepository>();
                //    commonRepository.UpdateAPIMethodDetail(apiLogID.ToString(), DateTime.Now.ToString(Constants.DateTimeForamt));
                //}
            }
            catch { }
            #endregion

        }
    }
}
