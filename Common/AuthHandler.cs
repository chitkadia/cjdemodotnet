using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Common
{
    public class AuthHandler : AuthorizationHandler<AccountHandler>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountHandler requirement)
        {
            var user = context.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }

    public class AccountHandler : IAuthorizationRequirement
    {
    }

    public enum Policy { Account };
}
