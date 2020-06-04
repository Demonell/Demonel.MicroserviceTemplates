using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                var interfaces = type.GetInterfaces().Where(t => t.IsGenericType).ToList();
                var mapFrom = interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IMapFrom<>));
                var mapTo = interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IMapTo<>));

                if (mapFrom != null || mapTo != null)
                {
                    var instance = Activator.CreateInstance(type);
                    var methodInfo = instance.GetType().GetMethod("Mapping");
                    if (methodInfo != null)
                    {
                        methodInfo?.Invoke(instance, new object[] {this});
                    }
                    else
                    {
                        if (mapFrom != null)
                        {
                            methodInfo = mapFrom.GetMethod("Mapping");
                            methodInfo?.Invoke(instance, new object[] {this});
                        }

                        if (mapTo != null)
                        {
                            methodInfo = mapTo.GetMethod("Mapping");
                            methodInfo?.Invoke(instance, new object[] {this});
                        }
                    }
                }
            }
        }
    }
}