using System;
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

    [HttpPost, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult>  Post([FromForm] PropostaAnexoModel model)
    {
      this._logger.LogInformation("Log.NovaPropostaModel", "Getting item {ID}", model);

      if (model.Id == null)
      {
        return BadRequest(new {Erro = "Aconteceu algo errado, tenta novamente!"});
      }
      var anexo = await Context.PropostaAnexos.FirstOrDefaultAsync(x =>x.Proposta.Id == model.Id);
      Context.PropostaAnexos.Remove(anexo);
      if(model.Anexo != null)
      {

        var proposta = await Context.Propostas.FirstOrDefaultAsync(x => x.Id == model.Id);
        using (Stream stream = model.Anexo.OpenReadStream())
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                var fileContent = binaryReader.ReadBytes((int)model.Anexo.Length);
                var propostaAnexo = new PropostaAnexo(fileContent, model.Anexo.FileName, model.Anexo.ContentType, proposta);
                await Context.PropostaAnexos.AddAsync(propostaAnexo);
            }
        }
      }
      await Context.PropostaAnexos.AddAsync(anexo);
      await Context.SaveChangesAsync();
      return Ok(new {Ok = true, Response="Arquivo alterado com suscesso!"});
    }
  }
}