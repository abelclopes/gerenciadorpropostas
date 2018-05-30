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
  [Route("api/[controller]")]
  public class PropostasController : BaseController
  {
    private readonly ILogger _logger;
    public PropostasController(IContext context, IMemoryCache memoryCache, ILogger<PropostasController> logger) : base(context, memoryCache)
    {
      this._logger = logger;
    }

    [HttpGet, Authorize]
    [SwaggerResponse(201,typeof(PagedList<Proposta>))]
    //[SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  ListaPaginada<PropostaModel> Get([FromQuery]PaginationParamsProposta model)
    {
      var listaPaginada = new ListaPaginada<PropostaModel>(model.PageNumber, model.PageSize);
      var propostas = new List<PropostaModel>();
      if(RestornaPropostaList().Any()){
        propostas = RestornaPropostaList();
        if(!string.IsNullOrEmpty(model.NomeProposta) || model.Valor > 0 && !string.IsNullOrEmpty(model.Valor.ToString()) || !string.IsNullOrEmpty(model.FornecedorID)){ 
          propostas = propostas.Where(x => x.NomeProposta.Contains(model.NomeProposta) 
                              ||  x.Valor.Equals(model.Valor)
                              ||  x.Fornecedor.Id == Guid.Parse(model.FornecedorID)
                              ||  x.Categoria.Id == Guid.Parse(model.CategoriaID)
                              ).ToList();
        }
        return listaPaginada.Carregar(propostas);        
      }
      return listaPaginada.Carregar(propostas);
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(PropostaModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult GetProposta(string id)
    {
      var propostas = new PropostaModel();
      if(!string.IsNullOrEmpty(id)){
        return Ok(RestornaPropostaList().FirstOrDefault(x => x.Id == Guid.Parse(id)));
      }
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }

    [HttpPost, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult>  Post([FromForm] NovaPropostaModel model)
    {
      this._logger.LogInformation("Log.NovaPropostaModel", "Getting item {ID}", model);

      if (model.NomeProposta == null)
      {
          return BadRequest(new {Erro = "aconteceu algo errado, tenta novamente!"});
      }
      // if(RestornaPropostaList().Any(x => x.NomeProposta == model.NomeProposta))
      // {
      //   throw new ArgumentException($"O Nome da Proposta {model.NomeProposta} já esta em uso");
      // }
       var proposta = new Proposta(model.NomeProposta,
                                 model.Descricao, model.Valor, 
                                ConsultaFornecedor(model.FornecedorID), 
                                ConsultaCategoria(model.CategoriaID), 
                                (PropostaStatus)Enum.ToObject(typeof(PropostaStatus),
                                 model.Status));

      await Context.Propostas.AddAsync(proposta);

      if(model.Anexo != null)
      {
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
      var usuario = Context.Usuarios.FirstOrDefault(x => x.Id == Guid.Parse(model.Usuario));
      var propostaHistorico = new PropostaHistorico(proposta, usuario );
      await Context.PropostasHistoricos.AddAsync(propostaHistorico);
      await Context.SaveChangesAsync();

      MemoryCache.Remove("propostas");

      return Ok(new {ok= "true", Response = "Proposta salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult> Put(string id, [FromBody] NovaPropostaModel model)
    {
      if (model == null ||  string.IsNullOrEmpty(id))
      {
          return BadRequest();
      }
      
      var proposta =  ConsultaProposta(id);
      if (proposta == null)
      {
          return NotFound();
      }

      var propost = new Proposta(model.NomeProposta, model.Descricao, model.Valor, 
                                ConsultaFornecedor(model.FornecedorID), 
                                ConsultaCategoria(model.CategoriaID), 
                                (PropostaStatus)Enum.ToObject(typeof(PropostaStatus) , model.Status)
                    );
      proposta.Atualizar(propost, Context);
      Context.Propostas.Update(proposta);
      await Context.SaveChangesAsync();

      MemoryCache.Remove("propostas");

      return Ok(new {Response = "Proposta atualizado com sucesso"});
    }
    
    [HttpDelete("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Delete(string id)
    {
      if(string.IsNullOrEmpty(id)){
          return BadRequest();
      }
      var user = ConsultaProposta(id);
      Context.Propostas.Remove(user);
      Context.SaveChanges();

      MemoryCache.Remove("propostas");
      return Ok(new {Response = "Proposta deletado com sucesso"});
    }
    // private List<Proposta> RestornaPropostaList(){
    //   return Context.Propostas.Where(x => !x.Excluido).ToList();
    // }
    private List<PropostaModel>  RestornaPropostaList(){
      return MemoryCache.GetOrCreate("propostas", entry =>
            {
              entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
              return Context.Propostas.Where(x => !x.Excluido)
                .Select(x => new PropostaModel
                { 
                  Id = x.Id,
                  NomeProposta = x.NomeProposta,
                  Descricao = x.Descricao,
                  Fornecedor = x.Fornecedor,
                  Categoria = x.Categoria,
                  Valor = x.Valor,
                  //PropostaHistorico = (x.PropostaHistorico.Any())? x.PropostaHistorico: null,
                  DataCriacao = x.DataCriacao,
                  Status = x.Status
                }).ToList();
            });
  }
    private Proposta ConsultaProposta(string id){
      return Context.Propostas.FirstOrDefault(x => x.Id == Guid.Parse(id) && !x.Excluido);
    }    
    private Fornecedor ConsultaFornecedor(string fornecedorID)
    {   
        return Context.Fornecedores.FirstOrDefault(x => x.Id == Guid.Parse(fornecedorID));           
    }
    private Categoria ConsultaCategoria(string categoriaID)
    {   
        return Context.Categorias.FirstOrDefault(x => x.Id == Guid.Parse(categoriaID));
    }
  }
}