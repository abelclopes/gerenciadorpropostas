
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using INFRAESTRUCTURE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute, IFilterFactory
    {
        private IErrorLog _logProvider;
        private IHttpContextAccessor _httpContextAccessor;

        public ExceptionFilter()
        { }

        private ExceptionFilter(IErrorLog logProvider, IHttpContextAccessor httpContextAccessor)
        {
            _logProvider = logProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logProvider = (IErrorLog)serviceProvider.GetService(typeof(IErrorLog));
            var httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));

            return new ExceptionFilter(logProvider, httpContextAccessor);
        }
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            _logProvider.Write(context.Exception.ToString());

            var errorStatusCode = context.Exception is ArgumentException 
            ? 400 
            : context.Exception is InvalidCredentialException 
                ? 401
                : 500;

            context.Result = new JsonResult(
                                            new Erro
                                            {
                                                Mensagem = context.Exception.Message,
                                                Status = errorStatusCode
                                            });

            _httpContextAccessor.HttpContext.Response.StatusCode = errorStatusCode;

            return base.OnExceptionAsync(context);
        }
    }
}