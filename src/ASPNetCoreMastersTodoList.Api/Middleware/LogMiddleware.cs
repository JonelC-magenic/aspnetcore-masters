using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.Middleware
{
    public class LogMiddleware
    {
        RequestDelegate _next;
        private readonly ILogger _logger;

        public LogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LogMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var reqLogStr = string.Empty;
            var resLogStr = string.Empty;
            try
            {
                // Log Request
                reqLogStr = await LogRequest(context.Request);

                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    // Temporary response body
                    context.Response.Body = responseBody;

                    // Continue Pipeline
                    await _next(context);

                    // Log Response
                    resLogStr = await LogResponse(context.Response);

                    // Copy Orignal body stream for result
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            finally
            {
                _logger.LogInformation($"Request: {reqLogStr}{Environment.NewLine}Response: {resLogStr}");
            }
        }

        private async Task<string> LogRequest(HttpRequest req)
        {
            var reqLogStr = "";

            try
            {
                var urlStr = "";
                var queryStr = "";
                var reqTypeStr = "";
                var headerStr = "";
                var bodyStr = "";

                // Get Request URL 
                urlStr = req.Path.Value;

                // Get Query string
                queryStr = req.QueryString.Value;

                // Get Request Type e.g. GET, POST etc.
                reqTypeStr = req.Method;

                // Get Request Headers
                foreach (var header in req.Headers)
                {
                    headerStr += string.Format("\"{0}\": [{1}]", header.Key, header.Value);
                }

                // Enable body to be read multiple times
                req.EnableBuffering();

                // Create buffer with same length of request
                var buffer = new byte[Convert.ToInt32(req.ContentLength)];
                
                // Copy body to buffer
                await req.Body.ReadAsync(buffer, 0, buffer.Length);

                // Convert buffer to string
                bodyStr = Encoding.UTF8.GetString(buffer);

                // Re-position request body pointer to beginning
                req.Body.Seek(0, SeekOrigin.Begin);

                // Log Request
                reqLogStr = $"Request: Url: {urlStr}, QueryString: {queryStr}, Method: {reqTypeStr}, Headers: {headerStr}, Body: {bodyStr}";
            }
            catch(Exception ex)
            {
                reqLogStr = $"Unable to log request. Error: {ex.ToString()}";
                _logger.LogError("Unable to log request.", ex);
            }

            return reqLogStr;
        }

        private async Task<string> LogResponse(HttpResponse res)
        {
            var resLogStr = "";

            try
            {
                var statusCodeStr = "";
                var headerStr = "";
                var bodyStr = "";

                statusCodeStr = res.StatusCode.ToString();

                // Get Response Headers
                foreach (var header in res.Headers)
                {
                    headerStr += string.Format("\"{0}\": [{1}]", header.Key, header.Value);
                }

                // Re-position response body pointer to beginning
                res.Body.Seek(0, SeekOrigin.Begin);

                // Read response body stream
                bodyStr = await new StreamReader(res.Body).ReadToEndAsync();

                // Re-position response body pointer to beginning
                res.Body.Seek(0, SeekOrigin.Begin);

                // Log Response
                resLogStr = $"Response: Status Code: {statusCodeStr}, Headers: {headerStr}, Body: {bodyStr}";
            }
            catch (Exception ex)
            {
                resLogStr = $"Unable to log response. Error: {ex.ToString()}";
                _logger.LogError("Unable to log response.", ex);
            }

            return resLogStr;
        }
    }

    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
