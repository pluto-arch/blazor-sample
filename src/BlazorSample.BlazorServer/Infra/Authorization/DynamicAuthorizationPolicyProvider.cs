﻿using Dotnetydd.Permission.Definition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;

namespace BlazorSample.BlazorServer;

public class DynamicAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    /// <inheritdoc />
    public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options,
        IPermissionDefinitionManager permissionDefinitionManager) : base(options)
    {
        _permissionDefinitionManager = permissionDefinitionManager;
    }

    /// <inheritdoc />
    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy != null)
        {
            return policy;
        }

        var permission = _permissionDefinitionManager.GetOrNull(policyName);

        if (permission != null)
        {
            var policyBuilder = new AuthorizationPolicyBuilder([]);
            policyBuilder.Requirements.Add(new OperationAuthorizationRequirement { Name = policyName });
            return policyBuilder.Build();
        }

        return null;
    }

}
