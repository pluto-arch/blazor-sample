﻿using Dotnetydd.Permission.Checker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BlazorSample.BlazorServer;

public class PermissionRequirementHandler : AuthorizationHandler<OperationAuthorizationRequirement>
{
    private readonly IPermissionChecker _permissionChecker;

    public PermissionRequirementHandler(IPermissionChecker permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }


    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
    {
        if (await _permissionChecker.IsGrantedAsync(context.User, requirement.Name))
        {
            context.Succeed(requirement);
        }
    }
}
