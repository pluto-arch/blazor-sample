﻿using BlazorSample.Application.Resources;
using Microsoft.Extensions.Localization;

namespace BlazorSample.Application.AppServices.Demo
{
    [Injectable(InjectLifeTime.Scoped,typeof(IDemoAppServices))]
    [AutoResolveDependency]
    public partial class DemoAppServices: IDemoAppServices
    {
        [AutoInject]
        private readonly IStringLocalizer<ModelSharedResource> _l;


        public string GetLocalString()
        {
            return _l[ModelSharedResource.Application,"not working!!!"];
        }
    }
}