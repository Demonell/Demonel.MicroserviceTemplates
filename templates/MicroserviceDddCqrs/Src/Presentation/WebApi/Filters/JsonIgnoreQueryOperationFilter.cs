using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Namotion.Reflection;
using Newtonsoft.Json;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace WebApi.Filters
{
    public class JsonIgnoreQueryOperationFilter : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var apiParameters = context.OperationDescription.Operation.Parameters;
            var contextualParameters = context.MethodInfo.GetParameters().Select(p => p.ToContextualParameter());

            foreach (var contextualParameter in contextualParameters)
            {
                if (HasFromQueryAttribute(contextualParameter))
                {
                    var properties = contextualParameter.Type.GetContextualProperties();
                    foreach (var contextualProperty in properties)
                    {
                        if (HasIgnoreAttribute(contextualProperty))
                            RemoveParameter(apiParameters, contextualProperty.Name);
                    }
                }
            }

            return true;
        }

        public bool HasFromQueryAttribute(ContextualParameterInfo contextualParameter)
        {
            return contextualParameter.ContextAttributes
                .GetAssignableToTypeName(nameof(FromQueryAttribute), TypeNameStyle.Name)
                .Any();
        }

        public bool HasIgnoreAttribute(ContextualPropertyInfo contextualProperty)
        {
            return contextualProperty.ContextAttributes
                .GetAssignableToTypeName(nameof(JsonIgnoreAttribute), TypeNameStyle.Name)
                .Any();
        }

        public void RemoveParameter(IList<OpenApiParameter> apiParameters, string parameterName)
        {
            var parameter = apiParameters.FirstOrDefault(p => p.Name.Equals(parameterName));
            if (parameter != null)
                apiParameters.Remove(parameter);
        }
    }
}