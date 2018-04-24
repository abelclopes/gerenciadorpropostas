
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.Security.Cryptography;

using Model;
using INFRAESTRUCTURE;
using DOMAIN;
using DOMAIN.EnumHelper;

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private IConfiguration _config;
    private readonly ApplicationDbContext _context;
    public AuthController(IConfiguration config, ApplicationDbContext context)
    {
      _config = config;
      _context = context;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult  Post([FromBody]LoginModel Login)
    {
        IActionResult  response = Unauthorized();
        var user = Authenticate(Login);

        if (user != null)
        {
            var tokenString = BuildToken(user);
            return Ok(new { token = tokenString });
        }

        return response;
    }
    private string BuildToken(UsuarioAuthModel user)
    {

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Nome),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Birthdate, user.DataNacimento.ToString("yyyy-MM-dd")),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Police),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Issuer"],
        claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
     private UsuarioAuthModel Authenticate(LoginModel login)
     {
        UsuarioAuthModel user = null;
        if(string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password)) return user;
        var encript = Util.GetSHA1HashData(login.Password);

        Usuario usuario = _context.Usuarios.FirstOrDefault(x => x.Email == login.Email && x.Senha == encript);

        if (!string.IsNullOrEmpty(usuario.Email))
        {
            user = new UsuarioAuthModel(){
                Email = usuario.Email,
                Nome = usuario.Nome,
                Police = EnumHelper.GetDescription(usuario.PerfilUsuario),
                DataNacimento = usuario.DataNacimento
            };
        }
        return user;
     }
  }
}