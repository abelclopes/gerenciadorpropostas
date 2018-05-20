
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Security.Cryptography;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


using Model;
using INFRAESTRUCTURE;
using DOMAIN;
using DOMAIN.EnumHelper;
using Swashbuckle.AspNetCore.SwaggerGen;
using DOMAIN.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using API.Model;

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class UsuariosController : BaseController
  {
    public UsuariosController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
    {}
   
    [HttpGet, Authorize]
    //[HttpGet, Authorize(Policy = "Administrador")]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  ListaPaginada<UsuariosModel> Get(PaginationParams model)
    {
      var usuarios = RestornaUsuariosList();
      if(!string.IsNullOrEmpty(model.buscaTermo)){ 
        usuarios = usuarios.Where(x => x.Nome.Contains(model.buscaTermo) 
                            ||  x.Cpf.Contains(model.buscaTermo)
                            ||  x.Email.Contains(model.buscaTermo)
                            ).ToList();
      }
      var listaPaginada = new ListaPaginada<UsuariosModel>(model.PageNumber, model.PageSize);
      return listaPaginada.Carregar(usuarios);
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
    public IActionResult Post([FromBody] NovoUsuarioModel model)
    {
       if (model == null)
      {
          return BadRequest();
      }
      if(RestornaUsuariosList().Any(x => x.Email == model.Email))
      {
        throw new ArgumentException($"O Email {model.Email} já esta em uso");
      }
      var permissaoUsuario = Context.PermissaoUsuarios.FirstOrDefault(x => x.Id == model.PermissaoId);
      Context.Usuarios.Add(new Usuario(model.Nome, model.Email, model.Cpf, model.DataNacimento, permissaoUsuario , model.Senha));
      Context.SaveChanges();

      MemoryCache.Remove("usuarios");
      return Ok(new {Response = "Usuário salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public IActionResult Put(string id, [FromBody] NovoUsuarioModel model)
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

      var permissaoUsuario = Context.PermissaoUsuarios.FirstOrDefault(x => x.Id == model.PermissaoId);      
      var user = new Usuario(model.Nome, model.Email, model.Cpf, model.DataNacimento, permissaoUsuario);
      if(!string.IsNullOrEmpty(model.Senha))
      {
        user.Senha = model.Senha;
      }
      usuario.Atualizar(user, Context);
      Context.Usuarios.Update(usuario);
      Context.SaveChanges();

      MemoryCache.Remove("usuarios");
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
      user.Excluido = true;
      // Context.Usuarios.Remove(user);
      Context.Usuarios.Update(user);
      Context.SaveChanges();

      MemoryCache.Remove("usuarios");
      return Ok(new {Response = "Usuário deletado com sucesso"});
    }

    private List<UsuariosModel>  RestornaUsuariosList(){
      return MemoryCache.GetOrCreate("usuarios", entry =>
                          {
                            entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                            return Context.Usuarios.Include(x => x.PermissaoUsuario).Include(x => x.PermissaoUsuario).Where(x => !x.Excluido)
                            .Select(x => new UsuariosModel
                              {
                                Id = x.Id,
                                Nome = x.Nome, 
                                Email = x.Email, 
                                Cpf = x.Cpf,
                                DataNacimento = x.DataNacimento,
                                PermissaoUsuario = x.PermissaoUsuario,
                                //PerfilDescricao = EnumHelper.GetDescription(x.PerfilUsuario), 
                              }).ToList();
          });
    }
    private Usuario ConsultaUsuario(string id){
      return Context.Usuarios.FirstOrDefault(x => x.Id == Guid.Parse(id) && !x.Excluido);
    }
  }
}