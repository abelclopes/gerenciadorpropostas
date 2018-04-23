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
      this.PageSize = 2;
    }


    [HttpGet, Authorize]
    public IActionResult  Get(PaginationParams model)
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
        return Ok(new PagedList<IUsuariosModel>(usuarios.AsQueryable(), this.PageNumber, this.PageSize));
      
      return Ok("Nenhum Resultado Encontrado");      
    }

    private void setPaginacao(PaginationParams model)
    {
        this.PageNumber = model.PageNumber;
        this.PageSize = model.PageSize;
    }

    [Route("{id}")]
    [HttpGet, Authorize]
    public IUsuariosModel GetUser(string id)
    {
      var usuarios = new IUsuariosModel();
      if(!string.IsNullOrEmpty(id)){
        return RestornaUsuariosList().FirstOrDefault(x => x.Id == Guid.Parse(id));
      }
      return usuarios;
    }
    //[Route("{id}")]
    [HttpPut("{id}")]//, Authorize]
    [ProducesResponseType(typeof(IUsuariosModel), 201)]
    [ProducesResponseType(typeof(IUsuariosModel), 400)]
    public IActionResult Put(string id, [FromBody] IUsuariosModel model)
    {
        if (model == null || model.Id.ToString() != id)
        {
            return BadRequest();
        }
        
        var usuario =  _context.Usuarios.FirstOrDefault(x => x.Id.ToString() == id);
        if (usuario == null)
        {
            return NotFound();
        }
        
        usuario = usuario.Atualizar(new Usuario(model.Nome, model.Cpf, model.DataNacimento, model.Police), _context);
        _context.Usuarios.Update(usuario);
        _context.SaveChanges();
        return new ObjectResult(usuario);
    }
    private List<IUsuariosModel>  RestornaUsuariosList(){
      return _context.Usuarios
      .Select(x => 
            new IUsuariosModel{ 
              Id = x.Id,
              Nome = x.Nome, 
              Email = x.Email, 
              Cpf = x.Cpf,
              DataNacimento = x.DataNacimento,
              Police = x.PerfilUsuario //EnumHelper.GetDescription(x.PerfilUsuario), 
            }).ToList();
    }
  }
}