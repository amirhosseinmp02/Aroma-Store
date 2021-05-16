using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Application.Security.Policy
{
    public class PolicyRequirement : IAuthorizationRequirement
    {
        public PolicyRequirement(string policyName)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; }
    }
}
