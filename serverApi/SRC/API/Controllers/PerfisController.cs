
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

using Swashbuckle.AspNetCore.SwaggerGen;
using StructureMap.Diagnostics;
using DOMAIN.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [Route("api/Usuarios/[controller]")]
    public class PerfisController : BaseController
    {
        public PerfisController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {}
        [HttpGet, Authorize]
        [SwaggerResponse(201)]
        [SwaggerResponse(401)]
        [SwaggerResponse(403)]
        public List<PerfilsModel> Get(){
            return new List<PerfilsModel>{
                new PerfilsModel(){id = 1, nome = "Administrador"},
                new PerfilsModel(){id = 2, nome = "Analista De Compras"},
                new PerfilsModel(){id = 3, nome = "Analista Financeiro"},
                new PerfilsModel(){id = 4, nome = "Diretor Financeiro"}
            };
        }
    }
}