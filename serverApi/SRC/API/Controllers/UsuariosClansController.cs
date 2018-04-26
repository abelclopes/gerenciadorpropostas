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
  public class UsuariosClansController : Controller
  {
    
    private readonly ApplicationDbContext _context;
    
    public UsuariosClansController(ApplicationDbContext context)
    {
      _context = context;    
    }


    [Route("{Email}")]
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(UsuarioAuthModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]
    public UsuarioAuthModel GetUser(string Email)
    {
      var usuarios = new UsuarioAuthModel();
      if(!string.IsNullOrEmpty(Email)){
        return ConsultaUsuario(Email);
      }
      return usuarios;
    }
    

    private UsuarioAuthModel ConsultaUsuario(string Email){
       var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == Email && !x.Excluido);
       
      return new UsuarioAuthModel{
          Nome = usuario.Nome,
          Email = usuario.Email,
          Police = EnumHelper.GetDescription(usuario.PerfilUsuario),
          Excluido = usuario.Excluido
      };
    }
  }
}