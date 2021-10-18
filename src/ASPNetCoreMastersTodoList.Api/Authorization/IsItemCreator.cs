using DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.Authorization
{
    public class IsItemCreatorRequirement : IAuthorizationRequirement { }

    public class IsItemCreatorHandler : AuthorizationHandler<IsItemCreatorRequirement, Item>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IsItemCreatorHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsItemCreatorRequirement requirement,
            Item resource)
        {
            var email = ((ClaimsIdentity)context.User.Identity).FindFirst("Email");
            var user = await _userManager.FindByNameAsync(email.Value);
            if (user is null) return;

            if (resource.CreatedBy == Guid.Parse(user.Id))
            {
                context.Succeed(requirement);
            }
        }
    }
}
