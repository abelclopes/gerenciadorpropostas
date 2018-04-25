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

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class CategoriasController : Controller
  {
    
    private int PageNumber;
    private int PageSize;
    private readonly ApplicationDbContext _context;
    
    public CategoriasController(ApplicationDbContext context)
    {
      _context = context;
      this.PageNumber = 1;
      this.PageSize = 20;
    }


    [HttpGet, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  IActionResult Get(PaginationParams model)
    {
      var categorias = RestornaCategoriaList();
      this.setPaginacao(model);
      if(!string.IsNullOrEmpty(model.buscaTermo)){ 
        categorias = categorias.Where(x => x.Nome.Contains(model.buscaTermo) 
                            ||  x.Descricao.Contains(model.buscaTermo)
                            ).ToList();
      }
      if(categorias.Count() > 0)
        return Ok(new PagedList<CategoriasModel>(categorias.AsQueryable(), this.PageNumber, this.PageSize));
      return Ok(new { Response = "Nenhum Resultado Encontrado" });
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
        return Ok(RestornaCategoriaList().FirstOrDefault(x => x.Id == Guid.Parse(id)));
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
      if(RestornaCategoriaList().Any(x => x.Descricao == model.Descricao))
      {
        throw new ArgumentException($"O Descricao {model.Descricao} já esta em uso");
      }
      var categoria = new Categoria(model.Nome, model.Descricao);
      _context.Categorias.Add(categoria);
      _context.SaveChanges();

      return Ok(new {Response = "Categoria salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Put(string id, [FromBody] NovaCategoriaModel model)
    {
      if (model == null ||  string.IsNullOrEmpty(id))
      {
          return BadRequest();
      }
      
      var categoria =  ConsultaCategoria(id);
      if (categoria == null)
      {
          return NotFound();
      }
      
      categoria.Atualizar( new Categoria(model.Nome, model.Descricao), _context);
      _context.Categorias.Update(categoria);
      _context.SaveChanges();

      return Ok(new {Response = "Categoria atualizado com sucesso"});
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
      var user = ConsultaCategoria(id);
      _context.Categorias.Remove(user);
      _context.SaveChanges();

      return Ok(new {Response = "Categoria deletado com sucesso"});
    }
    private List<CategoriasModel>  RestornaCategoriaList(){
      return _context.Categorias      
      .Where(x => !x.Excluido)
      .Select(x => 
            new CategoriasModel{ 
              Id = x.Id,
              Nome = x.Nome,
              Descricao = x.Descricao
            }).ToList();
    }
    private Categoria ConsultaCategoria(string id){
      return _context.Categorias.FirstOrDefault(x => x.Id == Guid.Parse(id) && !x.Excluido);
    }    
    private void setPaginacao(PaginationParams model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }
  }
}