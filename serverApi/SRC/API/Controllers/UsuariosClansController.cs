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
  
  [Route("api/usuarios/Clans")]
  public class UsuariosClansController : BaseController
  {    
    public UsuariosClansController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
    {}
   
    [HttpGet("{email}"), Authorize]
    [ProducesResponseType(typeof(UsuarioAuthModel), 201)]
    [SwaggerResponse(401)]
    [SwaggerResponse(403)]   
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
       return Context.Usuarios.Select(x => new UsuarioAuthModel{
          Id = x.Id,
          Nome = x.Nome,
          Cpf = x.Cpf,
          DataNacimento = x.DataNacimento,
          Idade = this.CalcularIdade(x.DataNacimento),
          Email = x.Email,
          Police = x.UsuarioPermissoes.Permissoes.Nome,
          Excluido = x.Excluido
      }).FirstOrDefault(x => x.Email == Email && !x.Excluido);     
    }
    private int CalcularIdade(DateTime nascimento)
    {
      var today = DateTime.Today;
      var idade = today.Year - nascimento.Year;
      if (nascimento > today.AddYears(-idade)) return idade--;
      return idade;
    }
  }
}