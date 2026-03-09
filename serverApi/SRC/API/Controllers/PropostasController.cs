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
using BUSINESS;
using API.Messaging;
using API.Messaging.Contracts;

namespace API.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class PropostasController : BaseController
  {
    private readonly ILogger _logger;
    private readonly IOutboxWriter _outboxWriter;
    public PropostasController(IContext context, IMemoryCache memoryCache, ILogger<PropostasController> logger, IOutboxWriter outboxWriter) : base(context, memoryCache)
    {
      this._logger = logger;
      _outboxWriter = outboxWriter;
    }

    [HttpGet, Authorize]
    [SwaggerResponse(201,typeof(ListaPaginada<PropostaPaginationModel>))]
    //[SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]

    public async Task<ListaPaginada<PropostaModel>> Get([FromQuery]PaginationParamsProposta model)
    {
      this.checaExistenciaDePropostasExpiradas();
      var pageNumber = model.PageNumber <= 0 ? 1 : model.PageNumber;
      var pageSize = model.PageSize <= 0 ? 10 : model.PageSize;
      var listaPaginada = new ListaPaginada<PropostaModel>(pageNumber, pageSize);
      var propostas = RestornaPropostaQueryable();

      if (!string.IsNullOrWhiteSpace(model.BuscaTermo))
      {
        var busca = model.BuscaTermo.Trim().ToLower();
        propostas = propostas.Where(x =>
          (x.NomeProposta ?? string.Empty).ToLower().Contains(busca) ||
          (x.Descricao ?? string.Empty).ToLower().Contains(busca) ||
          (x.Valor ?? string.Empty).ToLower().Contains(busca) ||
          (x.Fornecedor.Nome ?? string.Empty).ToLower().Contains(busca) ||
          (x.Categoria.Nome ?? string.Empty).ToLower().Contains(busca));
      }

      if (!string.IsNullOrWhiteSpace(model.NomeProposta))
      {
        var nome = model.NomeProposta.Trim().ToLower();
        propostas = propostas.Where(x => (x.NomeProposta ?? string.Empty).ToLower().Contains(nome));
      }

      if (!string.IsNullOrWhiteSpace(model.Descricao))
      {
        var descricao = model.Descricao.Trim().ToLower();
        propostas = propostas.Where(x => (x.Descricao ?? string.Empty).ToLower().Contains(descricao));
      }

      if (!string.IsNullOrWhiteSpace(model.Valor))
      {
        propostas = propostas.Where(x => (x.Valor ?? string.Empty).Contains(model.Valor));
      }

      if (!string.IsNullOrWhiteSpace(model.FornecedorID) && Guid.TryParse(model.FornecedorID, out Guid fornecedorId))
      {
        propostas = propostas.Where(x => x.Fornecedor.Id == fornecedorId);
      }

      if (!string.IsNullOrWhiteSpace(model.FornecedorNome))
      {
        var fornecedorNome = model.FornecedorNome.Trim().ToLower();
        propostas = propostas.Where(x => (x.Fornecedor.Nome ?? string.Empty).ToLower().Contains(fornecedorNome));
      }

      if (!string.IsNullOrWhiteSpace(model.CategoriaID) && Guid.TryParse(model.CategoriaID, out Guid categoriaId))
      {
        propostas = propostas.Where(x => x.Categoria.Id == categoriaId);
      }

      if (!string.IsNullOrWhiteSpace(model.CategoriaNome))
      {
        var categoriaNome = model.CategoriaNome.Trim().ToLower();
        propostas = propostas.Where(x => (x.Categoria.Nome ?? string.Empty).ToLower().Contains(categoriaNome));
      }

      if (model.Status.HasValue)
      {
        propostas = propostas.Where(x => (int)x.Status == model.Status.Value);
      }

      var sortBy = (model.SortBy ?? "dataCriacao").Trim().ToLower();
      var sortDir = (model.SortDir ?? "desc").Trim().ToLower();
      var isAsc = sortDir == "asc";

      switch (sortBy)
      {
        case "nome":
        case "nomeproposta":
          propostas = isAsc ? propostas.OrderBy(x => x.NomeProposta) : propostas.OrderByDescending(x => x.NomeProposta);
          break;
        case "fornecedor":
          propostas = isAsc ? propostas.OrderBy(x => x.Fornecedor.Nome) : propostas.OrderByDescending(x => x.Fornecedor.Nome);
          break;
        case "categoria":
          propostas = isAsc ? propostas.OrderBy(x => x.Categoria.Nome) : propostas.OrderByDescending(x => x.Categoria.Nome);
          break;
        case "valor":
          propostas = isAsc ? propostas.OrderBy(x => x.Valor) : propostas.OrderByDescending(x => x.Valor);
          break;
        case "status":
          propostas = isAsc ? propostas.OrderBy(x => x.Status) : propostas.OrderByDescending(x => x.Status);
          break;
        default:
          propostas = isAsc ? propostas.OrderBy(x => x.DataCriacao) : propostas.OrderByDescending(x => x.DataCriacao);
          break;
      }

      return await listaPaginada.Carregar(propostas);
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
        return Ok(RestornaPropostaQueryable().FirstOrDefault(x => x.Id == Guid.Parse(id)));
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
      model.Status = 1;
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

      await _outboxWriter.EnqueueAsync(
        messageType: nameof(PropostaCriadaIntegrationEvent),
        routingKey: "proposta.criada",
        payload: new PropostaCriadaIntegrationEvent
        {
          PropostaId = proposta.Id,
          NomeProposta = proposta.NomeProposta,
          Valor = proposta.Valor,
          Status = (int)proposta.Status,
          FornecedorId = proposta.FornecedorId,
          CategoriaId = proposta.CategoriaId,
          UsuarioId = usuario?.Id ?? Guid.Empty,
          CriadoEmUtc = DateTime.UtcNow
        }
      );

      await Context.SaveChangesAsync();

      MemoryCache.Remove("propostas");

      return Ok(new {ok= "true", Response = "Proposta salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult> Put(string id, [FromForm] NovaPropostaModel model)
    {
      this._logger.LogInformation("Log.NovaPropostaModel", "update item {ID}", id);
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
                                 (PropostaStatus)Enum.ToObject(typeof(PropostaStatus),
                                model.Status));
      
      var usuario = Context.Usuarios.FirstOrDefault(x => x.Id == Guid.Parse(model.Usuario));
      var propostaHistorico = new PropostaHistorico(proposta, usuario );
      await Context.PropostasHistoricos.AddAsync(propostaHistorico);
      proposta.Atualizar(propost);
      
      if(model.Anexo != null)
      {
        using (Stream stream = model.Anexo.OpenReadStream())
        {
            using (var binaryReader = new BinaryReader(stream))
            {
              var anexo = await Context.PropostaAnexos.FirstOrDefaultAsync(x => x.Proposta.Id == Guid.Parse(id));
              anexo.Excluido = true;
              Context.PropostaAnexos.Update(anexo);
              var fileContent = binaryReader.ReadBytes((int)model.Anexo.Length);
              var propostaAnexo = new PropostaAnexo(fileContent, model.Anexo.FileName, model.Anexo.ContentType, proposta);                
              await Context.PropostaAnexos.AddAsync(propostaAnexo);
            }
        }
      }
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
    private IQueryable<PropostaModel> RestornaPropostaQueryable(){
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

    
    private void checaExistenciaDePropostasExpiradas()
    {
      var pb = new PropostaBusiness();
      var propostas = Context.Propostas.Where(x => x.Status != (PropostaStatus)Enum.ToObject(typeof(PropostaStatus), 3) || x.Status != (PropostaStatus)Enum.ToObject(typeof(PropostaStatus), 1) && !x.Excluido);
      bool salvar = false;
      foreach(var proposta in propostas)
      {
        var usuario = Context.Usuarios.FirstOrDefault(x => x.UsuarioPermissoes.Permissoes.Nivel.Equals(1));
        bool valida = (!Context.PropostasHistoricos.Any(x => proposta.Id == x.PropostaId && x.PropostaStatus == (PropostaStatus)3 || x.PropostaStatus == (PropostaStatus)2));
        if(valida && pb.validaSePropsotaExpirou(proposta))
        {          
          salvar = true;
          proposta.Status =  (PropostaStatus)3;
          var propostaHistorico = new PropostaHistorico(proposta, usuario );
          Context.PropostasHistoricos.Add(propostaHistorico);
          Context.Propostas.Update(proposta);
          
        }
      }
      if(salvar)
      {
        MemoryCache.Remove("propostas");
        Context.SaveChanges();
      }
    }
    private float convertoToFloat(double input){
      float result = (float) input;
      if (float.IsPositiveInfinity(result))
      {
          result = float.MaxValue;
      } else if (float.IsNegativeInfinity(result))
      {
          result = float.MinValue;
      }
      return result;
    }
  }
}
