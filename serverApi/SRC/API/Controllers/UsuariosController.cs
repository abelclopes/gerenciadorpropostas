
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
using System.Globalization;

namespace API.Controllers
{
  [Authorize]
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class UsuariosController : BaseController
  {
    public UsuariosController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
    {}
   
    [HttpGet, Authorize]
    //[HttpGet]
    [ProducesResponseType(typeof(ListaPaginada<UsuariosModel>), 201)]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public  ListaPaginada<UsuariosModel> Get([FromQuery] PaginationParams model)
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
    public async Task<IActionResult> Post([FromBody] UpdateUsuarioModel model)
    {
       if (model == null)
      {
          return BadRequest();
      }
      if(RestornaUsuariosList().Any(x => x.Email == model.Email))
      {
        throw new ArgumentException($"O Email {model.Email} já esta em uso");
      }
      var Permissao = Context.Permissoes.FirstOrDefault(x => x.Nivel == model.perfilUsuario);
      var usuario = new Usuario(model.Nome, model.Email, model.Cpf, Util.convertDateTime(model.DataNacimento), model.Senha);
      var usuarioPermissoes = new UsuarioPermissao(usuario, Permissao);

      Context.Usuarios.Add(usuario);
      
      Context.UsuarioPermissoes.Add(usuarioPermissoes);
      await Context.SaveChangesAsync();

      MemoryCache.Remove("usuarios");
      return Ok(new {ok=true,Response = "Usuário salvo com sucesso"});
    }
    
    [HttpPut("{id}"), Authorize]
    [SwaggerResponse(201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateUsuarioModel model)
    {
      model.Id = Guid.Parse(id);
      if (!Context.Usuarios.Any(x => x.Id == Guid.Parse(id)))
      {
          return NotFound();
      }
      if(string.IsNullOrEmpty(model.Nome) || string.IsNullOrEmpty(model.Email)) return NotFound(new {error ="Nome ou email nao podem ser alterados para nulos"});
      var usuario =  ConsultaUsuario(id);    

      
      var user = new Usuario();
      user.Nome = model.Nome;
      user.Email = model.Email;
      user.Cpf =  model.Cpf.Replace(".","").Replace("-","");
      user.DataNacimento = Util.convertDateTime(model.DataNacimento);
      string dateInput = model.DataNacimento;
      
      if(!string.IsNullOrEmpty(model.Senha))
      {
        user.Senha = model.Senha;
      }
      var permissao = getPermissao().FirstOrDefault(x => x.Nivel.Equals(model.perfilUsuario));
      usuario.Atualizar(user, Context);
      if(!Context.UsuarioPermissoes.Any(x => x.UsuarioId == model.Id && x.PermissaoId == permissao.Id )){
        var up = Context.UsuarioPermissoes.FirstOrDefault(x => x.UsuarioId == model.Id && x.PermissaoId == permissao.Id );
        Context.UsuarioPermissoes.Remove(up);
        usuario.UsuarioPermissoes = new UsuarioPermissao(usuario, permissao);
      }
      Context.Usuarios.Update(usuario);
      await Context.SaveChangesAsync();

      MemoryCache.Remove("usuarios");
      return Ok(new {ok = true, Response = "Usuário salvo com sucesso"});
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
      return Ok(new {ok= true, Response = "Usuário deletado com sucesso"});
    }

    private List<UsuariosModel>  RestornaUsuariosList(){
      return MemoryCache.GetOrCreate("usuarios", entry =>
          {
            entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
            return Context.Usuarios.Where(x => !x.Excluido)
            .Select(x => new UsuariosModel
              {
                Id = x.Id,
                Nome = x.Nome, 
                Email = x.Email, 
                Cpf = x.Cpf,
                DataNacimento = x.DataNacimento,
                DataCriacao = x.DataCriacao,
                Permissao = x.UsuarioPermissoes.Permissoes.Nome,
                PermissaoNivel = x.UsuarioPermissoes.Permissoes.Nivel
              }).ToList();
          });
    }
    private Usuario ConsultaUsuario(string id){
      return Context.Usuarios.FirstOrDefault(x => x.Id == Guid.Parse(id) && !x.Excluido);
    }
    private List<Permissao> getPermissao() => Context.Permissoes.ToList();
    private DateTime? convertData(string value){
      
      DateTime convertedDate = Convert.ToDateTime(value);
      string date = convertedDate.ToShortDateString();
      return  DateTime.ParseExact(date, "dd/MM/yyyy", null);
    }



  }
    
}