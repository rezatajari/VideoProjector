﻿using VideoProjector.Common;

namespace VideoProjector.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ErrorHandling(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Error(exception, messageTemplate: "An unhandled exception occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ResponseCenter.CreateErrorResponse<object>(
                message: "An internal server error occurred. Please try again later.",
                errorCode: "INTERNAL_SERVER_ERROR"
            );

            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
