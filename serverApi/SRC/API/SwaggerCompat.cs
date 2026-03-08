using System;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class SwaggerResponseAttribute : Swashbuckle.AspNetCore.Annotations.SwaggerResponseAttribute
    {
        public SwaggerResponseAttribute(int statusCode)
            : base(statusCode)
        {
        }

        public SwaggerResponseAttribute(int statusCode, Type type)
            : base(statusCode, type: type)
        {
        }

        public SwaggerResponseAttribute(int statusCode, string description)
            : base(statusCode, description)
        {
        }

        public SwaggerResponseAttribute(int statusCode, string description, Type type)
            : base(statusCode, description, type)
        {
        }
    }
}
