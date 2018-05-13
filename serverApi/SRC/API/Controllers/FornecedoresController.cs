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
using API.Model;

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class FornecedoresController : BaseController
  {
    
    private int PageNumber;
    private int PageSize;
    public FornecedoresController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache){}


    [HttpGet, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  ListaPaginada<FornecedorModel> Get(PaginationParams model)
    {
      var fornecedor = RestornaFornecedorList();
      if(!string.IsNullOrEmpty(model.buscaTermo))
      {
        fornecedor = fornecedor.Where(x => x.Nome.ToLower().Contains(model.buscaTermo.ToLower()) || x.CnpjCpf.Contains(model.buscaTermo.ToLower())).ToList();
      }
      var listaPaginada = new ListaPaginada<FornecedorModel>(model.PageNumber, model.PageSize);
      return listaPaginada.Carregar(fornecedor);
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(FornecedorModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult GetFornecedor(string id)
    {
      var Fornecedors = new FornecedorModel();
      if(!string.IsNullOrEmpty(id) && RestornaFornecedorList().Any(x => x.Id == Guid.Parse(id))){
        return Ok(RestornaFornecedorList().FirstOrDefault(x => x.Id == Guid.Parse(id)));
      }
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }
    

    [Route("GetAll/{busca}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(FornecedorModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult GetBuscarFornecedor([FromRoute]string busca)
    {
      var Fornecedors = new FornecedorModel();
      if(!string.IsNullOrEmpty(busca)){
        var forncedores = RestornaFornecedorList().Where(x => x.CnpjCpf.Contains(busca) 
        || x.Nome.Contains(busca) || x.Email.Contains(busca) || x.Telefone.Contains(busca)).ToList();
        return Ok(new {Ok="sucesso", Response = forncedores, total = forncedores.Count()});
      }
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }

    [HttpPost, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Post([FromBody] NovoFornecedorModel model)
    {
       if (model == null)
      {
          return BadRequest();
      }
      if(RestornaFornecedorList().Any(x => x.Email == model.Email))
      {
        throw new ArgumentException($"O Email {model.Email} já esta em uso");
      }
      
      Context.Fornecedores.Add(new Fornecedor(model.Nome, model.Email, model.CnpjCpf, model.Telefone));
      Context.SaveChanges();

      MemoryCache.Remove("fornecedor");
      return Ok(new {ok= true, Response = "Fornecedor salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Put(string id, [FromBody] NovoFornecedorModel model)
    {
      if (model == null ||  string.IsNullOrEmpty(id))
      {
          return BadRequest();
      }
      
      var fornecedor =  ConsultaFornecedor(id);
      if (fornecedor == null)
      {
          return NotFound();
      }
      
      var fornec = new Fornecedor(model.Nome, model.Email, model.CnpjCpf, model.Telefone);
      fornecedor.Atualizar(fornec, Context);
      Context.Fornecedores.Update(fornecedor);
      Context.SaveChanges();

      MemoryCache.Remove("fornecedor");
      return Ok(new {ok=true, Response = "Fornecedor atualizado com sucesso"});
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
      var forn = ConsultaFornecedor(id);
      forn.Excluido = true;
      Context.Fornecedores.Update(forn);
      Context.SaveChanges();
      MemoryCache.Remove("fornecedor");
      return Ok(new {ok= true, Response = "Fornecedor deletado com sucesso"});
    }
   private List<FornecedorModel>  RestornaFornecedorList(){
      return MemoryCache.GetOrCreate("fornecedor", entry =>
                          {
                            entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                            return Context.Fornecedores.Where(x => !x.Excluido)
                            .Select(x => new FornecedorModel
                              {
                                Id = x.Id,
                                Nome = x.Nome,
                                CnpjCpf = x.CnpjCpf,
                                Email = x.Email,
                                Telefone = x.Telefone
                              }).ToList();
                          });
  }
    private Fornecedor ConsultaFornecedor(string id){
      var Uid = Guid.Parse(id.ToUpper());
      return Context.Fornecedores.FirstOrDefault(x => x.Id == Uid && !x.Excluido);
    }
    private void setPaginacao(PaginationParams model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }
  }
}