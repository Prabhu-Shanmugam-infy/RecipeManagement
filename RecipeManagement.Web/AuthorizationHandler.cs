﻿using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace RecipeManagement.Web
{
    public class AuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext ??
                throw new InvalidOperationException("No HttpContext available!");

            //var accessToken = await httpContext.GetTokenAsync("access_token");
            var accessToken = httpContext.Session.GetString("Token");

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
