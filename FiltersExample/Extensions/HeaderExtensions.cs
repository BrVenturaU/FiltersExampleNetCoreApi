using Microsoft.Extensions.Primitives;
using System.Reflection;

namespace FiltersExample.Extensions
{
    public static class HeaderExtensions
    {
        public static string GetTransactionId<THeader>(this HttpContext httpContext)
            where THeader : class
        {
            var transactionId = GetHeaderByName<THeader>(httpContext, "transactionId");

            return string.IsNullOrEmpty(transactionId) ? Guid.NewGuid().ToString() : transactionId;
        }

        public static string GetApplicationSource<THeader>(this HttpContext httpContext)
            where THeader : class
        {
            var applicationSource = GetHeaderByName<THeader>(httpContext, "applicationSource");
            return string.IsNullOrEmpty(applicationSource) ? Assembly.GetExecutingAssembly().GetName().Name : applicationSource;
        }

        public static string GetHeaderByName<THeader>(this HttpContext httpContext, string name)
            where THeader : class
        {
            StringValues value = string.Empty;
            if (typeof(THeader) == typeof(HttpRequest)) {
                httpContext.Request.Headers.TryGetValue(name, out StringValues headerValue);
                value = headerValue.FirstOrDefault();
            }
            if (typeof(THeader) == typeof(HttpResponse))
            {
                httpContext.Response.Headers.TryGetValue(name, out StringValues headerValue);
                value = headerValue.FirstOrDefault();
            }

            return value;
        }

    }
}
