using FiltersExample.Extensions;
using FiltersExample.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace FiltersExample.Filters
{
    public class MetadaActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isStrongTypedArgument = TryGetActionArgument(context, out KeyValuePair<string, object?> strongTypeArgument, p => p.Value is AppMetadata);

            var transactionId = context.HttpContext.GetTransactionId<HttpRequest>();
            var applicationSource = context.HttpContext.GetApplicationSource<HttpRequest>();

            if (isStrongTypedArgument)
            {
                SetValuesForStrongType(strongTypeArgument, transactionId, applicationSource);
                return;
            }

            var isStrongLessArgument = TryGetActionArgument(context, out KeyValuePair<string, object?> strongLessArgument, p =>
            {
                var propertyNames = p.Value?.GetType().GetProperties().Select(prop => prop.Name);
                var hasMetadataProperties = propertyNames.Any(prop => prop.Equals("TransactionId") || prop.Equals("ApplicationSource"));
                return hasMetadataProperties;
            });

            if (!isStrongLessArgument)
                return;

            SetValuesForStrongLessType(strongLessArgument, transactionId, applicationSource);

        }

        public bool TryGetActionArgument(ActionExecutingContext context, out KeyValuePair<string, object?> argument, Func<KeyValuePair<string, object?>, bool> predicate)
        {
            argument = context.ActionArguments.SingleOrDefault(predicate);
            return argument.Value != null;
        }

        public void SetValuesForStrongType(KeyValuePair<string, object?> argument, string transactionId, string applicationSource)
        {
            var meta = argument.Value as AppMetadata;
            meta.TransactionId = transactionId;
            meta.ApplicationSource = applicationSource;
        }

        public void SetValuesForStrongLessType(KeyValuePair<string, object?> argument, string transactionId, string applicationSource)
        {
            var argumentType = argument.Value.GetType();
            var properties = argumentType.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "TransactionId")
                    property.SetValue(argument.Value, transactionId);
                else if (property.Name == "ApplicationSource")
                    property.SetValue(argument.Value, applicationSource);
            }
        }
    }
}
