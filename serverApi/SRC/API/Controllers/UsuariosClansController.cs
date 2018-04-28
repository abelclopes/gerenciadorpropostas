﻿using System;
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


    [Route("{Email}")]
    [HttpPost, Authorize]
    [SwaggerResponse(401)]
    [SwaggerResponse(200)]
    public IActionResult GetUser(string Email)
    {
      var usuarios = new UsuarioAuthModel();
      if(!string.IsNullOrEmpty(Email)){
        return Ok(ConsultaUsuario(Email));
      }
      return BadRequest("nao encontrado");
    }
    

    private UsuarioAuthModel ConsultaUsuario(string Email){
       var usuario = Context.Usuarios.FirstOrDefault(x => x.Email == Email && !x.Excluido);
       
      return new UsuarioAuthModel{
          Nome = usuario.Nome,
          Email = usuario.Email,
          Police = EnumHelper.GetDescription(usuario.PerfilUsuario),
          Excluido = usuario.Excluido
      };
    }
  }
}