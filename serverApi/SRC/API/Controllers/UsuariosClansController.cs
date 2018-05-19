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

namespace API.Controllers
{
  
  [Route("api/[controller]")]
  public class UsuariosClansController : BaseController
  {    
    public UsuariosClansController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
    {}
   
    [HttpGet("{email}")]
    [Authorize]
    public IActionResult get(string email)
    {
      var usuarios = new UsuarioAuthModel();
      if (string.IsNullOrEmpty(email))
      {
          return BadRequest();
      }
      return Ok(ConsultaUsuario(email));      
      
    }
    

    private UsuarioAuthModel ConsultaUsuario(string Email){
       var usuario = Context.Usuarios.Include(x => x.PermissaoUsuario).FirstOrDefault(x => x.Email == Email && !x.Excluido);
       
      return new UsuarioAuthModel{
          Id = usuario.Id,
          Nome = usuario.Nome,
          Email = usuario.Email,
          Police = usuario.PermissaoUsuario.Permissao,
          Excluido = usuario.Excluido
      };
    }
  }
}