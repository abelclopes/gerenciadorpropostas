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
  public class UsuariosController : Controller
  {
    
    private int PageNumber;
    private int PageSize;
    private readonly ApplicationDbContext _context;
    
    public UsuariosController(ApplicationDbContext context)
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
      var usuarios = RestornaUsuariosList();
      this.setPaginacao(model);
      if(!string.IsNullOrEmpty(model.buscaTermo)){ 
        usuarios = usuarios.Where(x => x.Nome.Contains(model.buscaTermo) 
                            ||  x.Cpf.Contains(model.buscaTermo)
                            ||  x.Email.Contains(model.buscaTermo)
                            ).ToList();
      }
      if(usuarios.Count() > 0)
        return Ok(new PagedList<UsuariosModel>(usuarios.AsQueryable(), this.PageNumber, this.PageSize));
      
      return Ok("Nenhum Resultado Encontrado");      
    }

    private void setPaginacao(PaginationParams model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(UsuariosModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public UsuariosModel GetUser(string id)
    {
      var usuarios = new UsuariosModel();
      if(!string.IsNullOrEmpty(id)){
        return RestornaUsuariosList().FirstOrDefault(x => x.Id == Guid.Parse(id));
      }
      return usuarios;
    }
    [HttpPost, Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Post([FromBody] NovoUsuarioMode model)
    {
       if (model == null)
      {
          return BadRequest();
      }
      if(RestornaUsuariosList().Any(x => x.Email == model.Email))
      {
        throw new ArgumentException($"O Email {model.Email} já esta em uso");
      }
      _context.Usuarios.Add(new Usuario(model.Nome, model.Cpf, model.DataNacimento, model.PerfilUsuario, model.Senha));
      _context.SaveChanges();

      return Ok(new {Response = "Usuário salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Put(string id, [FromBody] NovoUsuarioMode model)
    {
      if (model == null ||  string.IsNullOrEmpty(id))
      {
          return BadRequest();
      }
      
      var usuario =  ConsultaUsuario(id);
      if (usuario == null)
      {
          return NotFound();
      }
      
      var user = new Usuario(model.Nome, model.Cpf, model.DataNacimento, model.PerfilUsuario);
      if(!string.IsNullOrEmpty(model.Senha))
      {
        user.Senha = model.Senha;
      }
      usuario.Atualizar(user, _context);
      _context.Usuarios.Update(usuario);
      _context.SaveChanges();

      return Ok(new {Response = "Usuário salvo com sucesso"});
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
      var user = ConsultaUsuario(id);
      _context.Usuarios.Remove(user);
      _context.SaveChanges();

      return Ok(new {Response = "Usuário deletado com sucesso"});
    }
    private List<UsuariosModel>  RestornaUsuariosList(){
      return _context.Usuarios
      .Select(x => 
            new UsuariosModel{ 
              Id = x.Id,
              Nome = x.Nome, 
              Email = x.Email, 
              Cpf = x.Cpf,
              DataNacimento = x.DataNacimento,
              Perfil = x.PerfilUsuario //EnumHelper.GetDescription(x.PerfilUsuario), 
            }).ToList();
    }

    private Usuario ConsultaUsuario(string id){
      return _context.Usuarios.FirstOrDefault(x => x.Id == Guid.Parse(id));
    }
  }
}