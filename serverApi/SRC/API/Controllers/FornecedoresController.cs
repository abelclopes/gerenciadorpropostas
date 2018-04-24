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
  public class FornecedoresController : Controller
  {
    
    private int PageNumber;
    private int PageSize;
    private readonly ApplicationDbContext _context;
    
    public FornecedoresController(ApplicationDbContext context)
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
      var fornecedors = RestornaFornecedorList();
      this.setPaginacao(model);
      if(!string.IsNullOrEmpty(model.buscaTermo)){ 
        fornecedors = fornecedors.Where(x => x.Nome.Contains(model.buscaTermo) 
                            ||  x.CnpjCpf.Contains(model.buscaTermo)
                            ||  x.Email.Contains(model.buscaTermo)
                            ).ToList();
      }
      if(fornecedors.Count() > 0)
        return Ok(new PagedList<FornecedorModel>(fornecedors.AsQueryable(), this.PageNumber, this.PageSize));
      
      return Ok("Nenhum Resultado Encontrado");      
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(FornecedorModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public FornecedorModel GetFornecedor(string id)
    {
      var Fornecedors = new FornecedorModel();
      if(!string.IsNullOrEmpty(id)){
        return RestornaFornecedorList().FirstOrDefault(x => x.Id == Guid.Parse(id));
      }
      return Fornecedors;
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
      
      _context.Fornecedores.Add(new Fornecedor(model.Nome, model.Email, model.CnpjCpf, model.Telefone));
      _context.SaveChanges();

      return Ok(new {Response = "Fornecedor salvo com sucesso"});
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
      fornecedor.Atualizar(fornec, _context);
      _context.Fornecedores.Update(fornecedor);
      _context.SaveChanges();

      return Ok(new {Response = "Fornecedor atualizado com sucesso"});
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
      var user = ConsultaFornecedor(id);
      _context.Fornecedores.Remove(user);
      _context.SaveChanges();

      return Ok(new {Response = "Fornecedor deletado com sucesso"});
    }
    private List<FornecedorModel>  RestornaFornecedorList(){
      return _context.Fornecedores
      .Select(x => 
            new FornecedorModel{ 
              Id = x.Id,
              Nome = x.Nome, 
              Email = x.Email, 
              CnpjCpf = x.CnpjCpf
            }).ToList();
    }

    private Fornecedor ConsultaFornecedor(string id){
      return _context.Fornecedores.FirstOrDefault(x => x.Id == Guid.Parse(id));
    }
    
    private void setPaginacao(PaginationParams model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }
  }
}