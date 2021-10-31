using ASPNetCoreMastersTodoList.Api.BindingModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.Middleware
{
    public class FormatResponseMiddleware
    {
        RequestDelegate _next;

        public FormatResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private Stream ReplaceBody(HttpResponse response)
        {
            var originBody = response.Body;
            response.Body = new MemoryStream();
            return originBody;
        }

        private async Task ReturnBody(HttpResponse response, Stream originBody)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            await response.Body.CopyToAsync(originBody);
            response.Body = originBody;
        }

        public async Task Invoke(HttpContext context)
        {
            Stream originBody = null;
            var responseModel = new ResponseBindingModel();

            try
            {
                originBody = ReplaceBody(context.Response);

                await _next(context);

                // Set Status Code
                responseModel.StatusCode = context.Response.StatusCode;

                // Re-position response body pointer to beginning
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string strBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                object objBody = null;

                try
                {
                    if (!string.IsNullOrEmpty(strBody))
                        objBody = JsonConvert.DeserializeObject(strBody);
                }
                catch
                {
                    objBody = null;
                    responseModel.ResultMessage = strBody;
                }
                
                // Read response body stream
                responseModel.ResultObject = objBody;

                // Re-position response body pointer to beginning
                context.Response.Body.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                // Set Status Code
                responseModel.StatusCode = 500;

                // Set error result message
                responseModel.ResultMessage = ex.ToString();
            }
            finally
            {
                // Get context type
                var contentType = context.Response.ContentType?.ToLower();
                contentType = contentType?.Split(';', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if (contentType == null)
                    contentType = "application/json";

                // Create new response body
                var requestContent = new StringContent(JsonConvert.SerializeObject(responseModel), Encoding.UTF8, contentType);
                context.Response.Body = await requestContent.ReadAsStreamAsync();
                context.Response.ContentLength = context.Response.Body.Length;


                await ReturnBody(context.Response, originBody);
            }
        }
    }

    public static class FormatResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseFormatResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FormatResponseMiddleware>();
        }
    }
}
