﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using Model;
using INFRAESTRUCTURE;
using DOMAIN;
using DOMAIN.EnumHelper;
using DOMAIN.Paginator;
using Swashbuckle.AspNetCore.SwaggerGen;
using StructureMap.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using API.Model;
using DOMAIN.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
  [Route("api/Propostas/Anexos/")]
  public class PropostaAnexosController : BaseController
  {
    private readonly ILogger _logger;
    public PropostaAnexosController(IContext context, IMemoryCache memoryCache, ILogger<PropostasController> logger) : base(context, memoryCache)
    {
      this._logger = logger;
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(PropostaAnexo), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult> GetProposta(string id)
    {
      var propostaAnexo = new PropostaAnexo();

      if(!string.IsNullOrEmpty(id) && await Context.PropostaAnexos.AnyAsync(x => x.Proposta.Id == Guid.Parse(id))){
        propostaAnexo = await Context.PropostaAnexos.FirstOrDefaultAsync(x => x.Proposta.Id == Guid.Parse(id));
        return Ok(propostaAnexo);
      }
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }
  }
}