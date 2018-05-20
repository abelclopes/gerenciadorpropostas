using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Model;
using INFRAESTRUCTURE;
using DOMAIN;
using DOMAIN.EnumHelper;
using DOMAIN.Paginator;
using Swashbuckle.AspNetCore.SwaggerGen;
using StructureMap.Diagnostics;
using DOMAIN.Interfaces;
using Microsoft.Extensions.Caching.Memory;


namespace API.Controllers
{
    [Route("api/Usuarios/Permissoes")]
    public class UsuariosPermissoesController : BaseController
    {
        public UsuariosPermissoesController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {}
        
        [HttpGet, Authorize]
        [SwaggerResponse(201)]
        [SwaggerResponse(401)]
        [SwaggerResponse(403)]
        public List<PermissaoUsuario> Get(){
            return Context.PermissaoUsuarios.Select(x => new PermissaoUsuario{
                Permissao = x.Permissao,
                Nivel = x.Nivel
            }).Where(x => !x.Excluido).ToList();
        }
    }
}