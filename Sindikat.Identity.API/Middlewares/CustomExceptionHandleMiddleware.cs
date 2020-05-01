using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sindikat.Identity.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sindikat.Identity.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            string result = null;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.ValidationErrors);
                    break;
                case UnauthorizedException _:
                    code = HttpStatusCode.Unauthorized;
                    result = "";
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    result = "";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == null)
            {
                var errorMessage = exception.Message;

                result = JsonConvert.SerializeObject(new { error = errorMessage });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
