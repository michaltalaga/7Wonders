using _7Wonders.Hubs;
using _7Wonders.Infrastructure;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;

[assembly: OwinStartupAttribute(typeof(_7Wonders.Startup))]
namespace _7Wonders
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer),() => JsonSerializerFactory.Value);
            DependencyInjection.BuildContainer();
			app.MapSignalR();
			GlobalHost.HubPipeline.AddModule(new ErrorHandlingModule());
        }
        private static readonly Lazy<JsonSerializer> JsonSerializerFactory = new Lazy<JsonSerializer>(GetJsonSerializer);
        private static JsonSerializer GetJsonSerializer()
        {
            return new JsonSerializer
            {
                ContractResolver = new FilteredCamelCasePropertyNamesContractResolver
                {
                    // 1) Register all types in specified assemblies:
                    AssembliesToInclude =
                    {
                        typeof (Startup).Assembly
                    },
                }
            };
        }
    }

    public class FilteredCamelCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public FilteredCamelCasePropertyNamesContractResolver()
        {
            AssembliesToInclude = new HashSet<Assembly>();
            TypesToInclude = new HashSet<Type>();
        }
        // Identifies assemblies to include in camel-casing
        public HashSet<Assembly> AssembliesToInclude { get; set; }
        // Identifies types to include in camel-casing
        public HashSet<Type> TypesToInclude { get; set; }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            Type declaringType = member.DeclaringType;
            if (
                TypesToInclude.Contains(declaringType)
                || AssembliesToInclude.Contains(declaringType.Assembly))
            {
                jsonProperty.PropertyName = ToCamelCase(jsonProperty.PropertyName); 
            }
            return jsonProperty;
        }
        public static string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            var firstChar = value[0];
            if (char.IsLower(firstChar))
            {
                return value;
            }
            firstChar = char.ToLowerInvariant(firstChar);
            return firstChar + value.Substring(1);
        }

    }
}
