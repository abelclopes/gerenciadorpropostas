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

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class PropostasController : BaseController
  {
    
    private int PageNumber;
    private int PageSize;
    public PropostasController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache){}


    [HttpGet, Authorize]
    [SwaggerResponse(201,typeof(ListaPaginada<PropostaPaginationModel>))]
    //[SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  ListaPaginada<PropostaPaginationModel> Get([FromQuery]PaginationParamsProposta model)
    {
      var listaPaginada = new ListaPaginada<PropostaPaginationModel>(model.PageNumber, model.PageSize);
      var propostas = new List<PropostaPaginationModel>();
      if(RestornaPropostaList().Any()){
        propostas = RestornaPropostaList();
        if(!string.IsNullOrEmpty(model.NomeProposta) || model.Valor > 0 && !string.IsNullOrEmpty(model.Valor.ToString()) || !string.IsNullOrEmpty(model.FornecedorID)){ 
          propostas = propostas.Where(x => x.NomeProposta.Contains(model.NomeProposta) 
                              ||  x.Valor.Equals(model.Valor)
                              ||  x.Fornecedor.Id == Guid.Parse(model.FornecedorID)
                              ||  x.Categoria.Id == Guid.Parse(model.CategoriaID)
                              ).ToList();                
        }
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

      if (model.NomeProposta == null)
      {
          return BadRequest(new {Erro = "aconteceu algo errado, tenta novamente!"});
      }
      if(RestornaPropostaList().Any(x => x.NomeProposta == model.NomeProposta))
      {
        throw new ArgumentException($"O Nome da Proposta {model.NomeProposta} já esta em uso");
      }
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
       await Context.SaveChangesAsync();

      return Ok(new {Response = "Proposta salvo com sucesso"});
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

      return Ok(new {Response = "Proposta deletado com sucesso"});
    }
     private List<PropostaPaginationModel>  RestornaPropostaList(){
      return MemoryCache.GetOrCreate("propostas", entry =>
                          {
                            entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                           return Context.Propostas.Where(x => !x.Excluido).Select(p => new PropostaPaginationModel{
                                Id = p.Id,
                                NomeProposta = p.NomeProposta,
                                Status = p.Status,
                                Valor = p.Valor,
                                CategoriaNome = p.Categoria.Nome,
                                CategoriaId = p.CategoriaId,
                                FornecedorNome = p.Fornecedor.Nome,
                                FornecedorId = p.FornecedorId,
                                DataCriacao = p.DataCriacao,
                                DataAtualizacao = p.DataAtualizacao,
                                Descricao = p.Descricao
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
    private void setPaginacao(PaginationParamsProposta model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }
  }
}