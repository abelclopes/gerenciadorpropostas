
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
    [Route("api/propostas/status")]
    public class PropostasStatusController : BaseController
    {
        public PropostasStatusController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {}
        [HttpGet, Authorize]
        [SwaggerResponse(201)]
        [SwaggerResponse(401)]
        [SwaggerResponse(403)]
        public List<PropostaStatusModel> Get(){
            return new List<PropostaStatusModel>{
                new PropostaStatusModel(){Id = 1, Status = "Aguardando Avaliação"},
                new PropostaStatusModel(){Id = 2, Status ="Aprovado"},
                new PropostaStatusModel(){Id = 3, Status = "Expirado"}
            };
        }
    }
}