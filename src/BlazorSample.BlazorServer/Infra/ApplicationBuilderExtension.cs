﻿using BlazorSample.Infra.Global;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Serilog;

namespace BlazorSample.BlazorServer;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 请求日志
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(config =>
        {
            config.EnrichDiagnosticContext = (context, httpContext) =>
            {
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    context.Set("x_forwarded_for", httpContext.Request.Headers["X-Forwarded-For"]);
                context.Set("request_path", httpContext.Request.Path);
                context.Set("request_method", httpContext.Request.Method);
            };
        });
        return app;
    }

    /// <summary>
    /// 用户访问器
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCurrentUserAccessor(this IApplicationBuilder app)
    {
        // 用户访问器
        app.Use((context, next) =>
        {
            var currentToken = context.RequestServices.GetService<GlobalAccessor.CurrentUser>();
            using (currentToken?.Change(context.User))
            {
                return next();
            }
        });
        return app;
    }

    public static IEndpointRouteBuilder MapSystemHealthChecks(this IEndpointRouteBuilder builer)
    {
        builer.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (c, r) =>
            {
                c.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(r.Entries);
                await c.Response.WriteAsync(result);
            }
        });
        return builer;
    }
}
