
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using INFRAESTRUCTURE;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Filters
{
    public class ModelValidateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                IEnumerable<string> erros = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);

                context.Result = new JsonResult(new Erro { Mensagem = String.Join(", ", erros) });
            }
        }
    }
}