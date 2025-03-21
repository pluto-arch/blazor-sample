﻿using System.Security.Claims;
using BlazorSample.Domain.Infra;

namespace BlazorSample.Infra.Global;

public class GlobalAccessor
{
    /// <summary>
    /// 全局用户访问器
    /// </summary>
    public class CurrentUserAccessor
    {
        private readonly AsyncLocal<ClaimsPrincipal> _tokenLocal = new();

        /// <summary>
        /// 当前用户身份
        /// </summary>
        public ClaimsPrincipal CurrentUser
        {
            get => _tokenLocal.Value!;
            set => _tokenLocal.Value = value;
        }
    }



    public class CurrentUser
    {
        private readonly CurrentUserAccessor _currentAccessor;

        public CurrentUser(CurrentUserAccessor currentTenantAccessor)
        {
            _currentAccessor = currentTenantAccessor;
        }


        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual string UserId => _currentAccessor.CurrentUser?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;


        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string UserName => _currentAccessor.CurrentUser?.Identity?.Name;


        /// <summary>
        /// 用户声明
        /// </summary>
        public virtual List<Claim> Claims => _currentAccessor.CurrentUser?.Claims?.ToList();



        public IDisposable Change(ClaimsPrincipal user)
        {
            var parentScope = _currentAccessor.CurrentUser;
            _currentAccessor.CurrentUser = user;
            return new DisposeAction(() =>
            {
                _currentAccessor.CurrentUser = parentScope;
            });
        }
    }
}