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
  [Authorize]
  [Route("api/[controller]")]
  public class CategoriasController : BaseController
  {
    
    private int PageNumber;
    private int PageSize;
    public CategoriasController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache){}


    [HttpGet, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public ListaPaginada<CategoriasModel> Get(PaginationParams model)
    {      
      var categorias = RestornaCategoriaList();
      if(!string.IsNullOrEmpty(model.buscaTermo))
      {
        categorias =  categorias.Where(x => x.Nome.ToLower().Contains(model.buscaTermo.ToLower())).ToList();
      }
      var listaPaginada = new ListaPaginada<CategoriasModel>(model.PageNumber, model.PageSize);
      return listaPaginada.Carregar(categorias);      
    }
    
    [Route("GetAll")]
    [HttpGet, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult GetCategorias()
    {
      var categorias = RestornaCategoriaList();
      if(categorias == null){
        return BadRequest(new {Error="Nenhuma categoria encontrada", Response = "Nenhum Resultado Encontrado" });
      }
      return Ok(new {ok="sucesso", Response = categorias});
    }
    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(CategoriasModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult GetCategoria(string id)
    {
      var categorias = new CategoriasModel();
      if(!string.IsNullOrEmpty(id)){
        return Ok(ConsultaCategoria(id));
      }
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
    }

    [HttpPost, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Post([FromBody] NovaCategoriaModel model)
    {
       if (model == null)
      {
          return BadRequest();
      }
      if(Context.Categorias.Any(x => x.Descricao == model.Descricao && !x.Excluido))
      {
        throw new ArgumentException($"O Descricao {model.Descricao} já esta em uso");
      }
      var categoria = new Categoria(model.Nome, model.Descricao);
      Context.Categorias.Add(categoria);
      Context.SaveChanges();

      MemoryCache.Remove("categorias");

      return Ok(new {OK = true, Response = "Categoria salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Put(string id, [FromQuery] NovaCategoriaModel model)
    {
      if (model.Nome == null ||  string.IsNullOrEmpty(id))
      {
          return BadRequest();
      }
      
      var categoria =  ConsultaCategoria(id);
      if (categoria == null)
      {
          return NotFound();
      }
      
      categoria.Atualizar( new Categoria(model.Nome, model.Descricao), Context);
      Context.Categorias.Update(categoria);
      Context.SaveChanges();

      MemoryCache.Remove("categorias");

      return Ok(new {ok = true, Response = "Categoria atualizado com sucesso"});
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
      var cat = ConsultaCategoria(id);
      cat.Excluido = true;
      Context.Categorias.Update(cat);
      Context.SaveChanges();

      MemoryCache.Remove("categorias");
      return Ok(new {ok = true, Response = "Categoria deletado com sucesso"});
    }
    private List<CategoriasModel>  RestornaCategoriaList(){
      return MemoryCache.GetOrCreate("categorias", entry =>
                            {
                              entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                              return Context.Categorias.Where(x => !x.Excluido)
                                .Select(x => new CategoriasModel
                                { 
                                  Id = x.Id,
                                  Nome = x.Nome,
                                  Descricao = x.Descricao,
                                  DataCriacao = x.DataCriacao
                                }).ToList();
                            });
    }
    private Categoria ConsultaCategoria(string id){
      var Uid = Guid.Parse(id.ToUpper());
      return Context.Categorias.FirstOrDefault(x => x.Id == Uid && !x.Excluido);
    }
    private void setPaginacao(PaginationParams model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }
  }
}