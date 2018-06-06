
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using INFRAESTRUCTURE;
using System;

namespace Filters
{
    public class ActivityLogFilter : ResultFilterAttribute, IFilterFactory
    {
        private readonly IActivityLog _log;

        public ActivityLogFilter()
        { }

        private ActivityLogFilter(IActivityLog log)
        {
            _log = log;
        }

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logProvider = (IActivityLog)serviceProvider.GetService(typeof(IActivityLog));
            return new ActivityLogFilter(logProvider);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _log.Write(context.Result.ToString());

            return base.OnResultExecutionAsync(context, next);
        }
        
    } 
}