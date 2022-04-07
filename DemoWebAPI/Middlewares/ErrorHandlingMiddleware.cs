
using Demo.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoWebAPI.Middlewares
{
    public class ErrorHandlingMiddleware: ExceptionFilterAttribute
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
              Stream originalBody = context.Response.Body;
                var path=  context.Request.Path.ToUriComponent();
                if(!(path.Contains("token") || path.Contains("Crawler"))){                    
                       if (TokenExpired(context.User))
                        {
                        APIResponseGeneric<dynamic> response = new APIResponseGeneric<dynamic>();
                            response.status = ApplicationMessage.Error;
                            response.message = ApplicationMessage.Unauthorized;
                            var mes = JsonConvert.SerializeObject(response);
                            await context.Response.WriteAsync(mes);
                            return;
                        }                    
                }

                if (context.Response.StatusCode==400)
                {
                    await HandleExceptionAsync(context, new Exception());
                }
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            APIResponseGeneric<dynamic> response = new APIResponseGeneric<dynamic>();
            response.status = ApplicationMessage.Error;
            response.message = ApplicationMessage.Error;
            response.data = exception.Message;
            var mes = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(mes);
        }
        private bool TokenExpired(ClaimsPrincipal claimsPrincipal)
        {
            bool expired = true;
            if (claimsPrincipal != null)
            {


                if (claimsPrincipal.HasClaim(c => c.Type == ClaimTypes.Expiration))
                {
                    DateTime dateTime;
                    if(DateTime.TryParse(
                    claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration).Value,out dateTime))
                    {
                        if (dateTime > DateTime.Now)
                        {
                            expired = false;
                        }
                    }
                }
            }
            return expired;
        }
    }
}
