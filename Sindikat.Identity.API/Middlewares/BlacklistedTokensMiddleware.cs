﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Sindikat.Identity.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Sindikat.Identity.API.Middlewares
{
    public class BlacklistedTokensMiddleware
    {
        private readonly RequestDelegate _next;

        public BlacklistedTokensMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IDistributedCache distributedCache, IJwtService jwtService)
        {
            var authorizationHeader = httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization");

            if (authorizationHeader.Value.Count > 0)
            {
                var bearerToken = authorizationHeader.Value.FirstOrDefault();

                var token = bearerToken.Substring("Bearer".Length + 1);

                var validatedToken = jwtService.GetPrincipalFromToken(token, validateLifetime: true);

                if (validatedToken == null)
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }

                var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                var blacklistedToken = distributedCache.Get(jti);

                if (blacklistedToken != null)
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }

                await _next(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }

    }
}