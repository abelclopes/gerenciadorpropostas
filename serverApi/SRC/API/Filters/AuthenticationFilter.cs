
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using INFRAESTRUCTURE;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DOMAIN.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Authentication;

namespace Filters
{
    // public class AuthenticationFilter : ActionFilterAttribute, IFilterFactory
    // {
    //     private IHttpContextAccessor _httpContextAccessor;
    //     private IContext _context;
    //     private IMemoryCache _memoryCache;
    //     public string permissao { get; set; }

    //     public AuthenticationFilter()
    //     {
    //         Order = 1;
    //     }

    //     private AuthenticationFilter(IHttpContextAccessor httpContextAccessor, IContext context, IMemoryCache memoryCache)
    //     {
    //         _httpContextAccessor = httpContextAccessor;
    //         _context = context;
    //         _memoryCache = memoryCache;
    //     }

    //     public bool IsReusable => false;

    //     public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    //     {
    //         IHttpContextAccessor httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
    //         IContext legislacaoContext = (IContext)serviceProvider.GetService(typeof(IContext));
    //         IMemoryCache memoryCache = (IMemoryCache)serviceProvider.GetService(typeof(IMemoryCache));

    //         return new AuthenticationFilter(httpContextAccessor, legislacaoContext, memoryCache);
    //     }

    //     override public void OnActionExecuting(ActionExecutingContext context)
    //     {
    //         var userLogin = _httpContextAccessor.HttpContext.User.Identity.Name;

    //         var userDb = _memoryCache.GetOrCreate($"permissoes-{userLogin}", entry =>
    //         {
    //             entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
    //             return _context.Usuarios.AsNoTracking()
    //                     .FirstOrDefault(x => x.Email.Equals(userLogin) && !x.Excluido);
    //         });

    //         if (userDb == null)
    //             throw new InvalidCredentialException($"Usuário {userLogin} não está registrado");

    //         var isAdmin = userDb.PerfilUsuario.Equals("Administrador");

    //         if (isAdmin)
    //             return;

    //         var userPermissions = userDb.PerfilUsuario.Equals("");

    //         if (UserDontHasPermission(userPermissions))
    //             throw new InvalidCredentialException($"Usuário {userLogin} não está autorizado à acessar esse recurso");

    //     }

    //     private bool UserDontHasPermission(IEnumerable<string> userPermissions)
    //     {
    //         return !userPermissions.Contains(permissao);
    //     }
    // }
}