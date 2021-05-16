using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Application.Security.Policy
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if ((requirement.PolicyName == "Founder") && (context.User.IsInRole("Founder")))
            {
                context.Succeed(requirement);
            }

            else if ((requirement.PolicyName == "Manager") && (context.User.IsInRole("Manager") || context.User.IsInRole("Founder")))
            {
                context.Succeed(requirement);
            }

            else if ((requirement.PolicyName == "Writer") && (context.User.IsInRole("Writer") || context.User.IsInRole("Manager") || context.User.IsInRole("Founder")))
            {
                context.Succeed(requirement);
            }

            else if ((requirement.PolicyName == "Customer") && (context.User.IsInRole("Customer") ||
                                                                context.User.IsInRole("Writer") ||
                                                                context.User.IsInRole("Manager") ||
                                                                context.User.IsInRole("Founder")))
            {

            }

            return Task.CompletedTask;
        }
    }
}
