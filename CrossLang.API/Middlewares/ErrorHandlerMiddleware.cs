using CrossLang.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrossLang.API.Middlewares
{
    /// <summary>
    /// Xử lý lỗi
    /// </summary>
    /// CreatedBy: VMHOANG
    public class ErrorHandlerMiddleware
    {

        #region DECLARE
        private readonly RequestDelegate next;
        #endregion

        #region Constructor
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                await HandleExceptionAsync(context, e);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            // Trả về 500 nếu có lỗi, kết quả không như mong đợi
            var code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(
                new ApiReturn
                {
                    UserMsg = Properties.Resources.API_ResponseMessage_Default,
                    ErrorCode = $"{Properties.Resources.API_ErrorCodePrefix}{CrossLang.ApplicationCore.Enums.MISACode.UnexpectedError}",
                    DevMsg = e.Message,
                    TraceId = context?.TraceIdentifier,
                    MoreInfo = e.Source
                }); 
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
        #endregion
    }
}