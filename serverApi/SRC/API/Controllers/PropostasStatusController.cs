
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
using BUSINESS;
using BUSINESS.Model;

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
        
        [HttpPost, Authorize]
        [SwaggerResponse(201)]
        [SwaggerResponse(401)]
        [SwaggerResponse(403)]
        public async Task<IActionResult> Post([FromBody]PropostaSituacaoModel model){

            var id = model.Id;
            var UsuarioId = Guid.Parse(model.UsuarioId);
            var usuarioLogado = Context.Usuarios.FirstOrDefault(x => x.Id == UsuarioId);
            var proposta = new Proposta();
            proposta = await Context.Propostas.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            var pb = new PropostaBusiness(Context);
            var status = (PropostaStatus)Enum.ToObject(typeof(PropostaStatus), proposta.Status);
            var propSituacao = pb.validate(proposta, usuarioLogado, status);
            var PermissaoId = Context.UsuarioPermissoes.FirstOrDefault(x => x.UsuarioId == UsuarioId).PermissaoId;
            var usuario = new NovoUsuarioModel(){
                    Email = usuarioLogado.Email,
                    Nome = usuarioLogado.Nome,
                    PermissaoId = PermissaoId,
                    Perfil = Context.Permissoes.FirstOrDefault(x => x.Id == PermissaoId).Nivel,
                    DataNacimento = usuarioLogado.DataNacimento
                };
            return Ok(new {Ok=true, Response =new {propSituacao, usuario}});
        }
        [HttpPut("{id}"), Authorize]
        [SwaggerResponse(201)]
        [SwaggerResponse(401)]
        [SwaggerResponse(403)]
        public async Task<IActionResult> Put(string id, [FromBody]PropostaSituacaoModel model)
        {
            if (string.IsNullOrEmpty(model.Status.ToString()))
            {
                return BadRequest(new {Response= "Não foi possivel aprovar a proposta"});
            }
            if (string.IsNullOrEmpty(model.UsuarioId.ToString()))
            {
                return BadRequest(new {Response= "Não foi possivel aprovar a proposta"});
            }

            var propsotaId = Guid.Parse(id);
            var proposta = await Context.Propostas.FirstOrDefaultAsync(x => x.Id == propsotaId 
            && x.Status != (PropostaStatus)Enum.ToObject(typeof(PropostaStatus), 3) 
            || x.Status != (PropostaStatus)Enum.ToObject(typeof(PropostaStatus), 1) && !x.Excluido);
            
            var usuario = await Context.Usuarios.FirstOrDefaultAsync(x => x.Id == Guid.Parse(model.UsuarioId));
            proposta.Status =  (PropostaStatus)model.Status;

            var propostaHistorico = new PropostaHistorico(proposta, usuario );

            await Context.PropostasHistoricos.AddAsync(propostaHistorico);
            Context.Propostas.Update(proposta);
            await Context.SaveChangesAsync();
            return Ok(new {ok = true,Response= "Proposta Aprovada"});
        }
    }
}