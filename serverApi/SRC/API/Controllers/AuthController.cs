
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

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private IConfiguration _config;
        public AuthController(IContext context, IMemoryCache memoryCache, IConfiguration config) : base(context, memoryCache)
        {
            _config = config;
        }
    

        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(201)]
        [SwaggerResponse(401)]
        public IActionResult  Post([FromBody]LoginModel Login)
        {
            var user = Authenticate(Login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                return Ok(new { token = tokenString });
            }
            return BadRequest("ERROR: Acesso Negado!, O e-mail ou senha invalidos");

        }
        private string BuildToken(NovoUsuarioModel user)
        {
            
            // var claims = new[] {
            //     new Claim(JwtRegisteredClaimNames.Sub, user.Nome),
            //     new Claim(JwtRegisteredClaimNames.Email, user.Email),
            //     new Claim(JwtRegisteredClaimNames.Birthdate, user.DataNacimento.ToString("yyyy-MM-dd")),
            //     new Claim(JwtRegisteredClaimNames.GivenName, user.PermissaoId.ToString()),
            //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // };

            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            // _config["Jwt:Issuer"],
            // claims,
            // expires: DateTime.Now.AddMinutes(120),
            // signingCredentials: creds);
        
            var model = TokenBuilder.CreateJsonWebToken(
                "gerp.project", 
                new List<string>() {user.PermissaoId.ToString(), user.Nome,user.Email,user.DataNacimento.ToString("yyyy-MM-dd"), user.PermissaoId.ToString() } ,
                "http://audience.com", 
                "http://issuer.com", 
                Guid.NewGuid(), 
                DateTime.UtcNow.AddMinutes(20)
            );
    
            return model;
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private NovoUsuarioModel Authenticate(LoginModel login)
        {
            NovoUsuarioModel user = null;
            if(string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password)) return user;
            var encript = Util.GetSHA1HashData(login.Password);

            Usuario usuario =  Context.Usuarios.Include(x => x.UsuarioPermissoes).FirstOrDefault(x => x.Email == login.Email && x.Senha == encript);
            if (!string.IsNullOrEmpty(usuario?.Email))
            {
                user = new NovoUsuarioModel(){
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    PermissaoId = usuario.UsuarioPermissoes.PermissaoId,
                    DataNacimento = usuario.DataNacimento
                };
            }
            return user;
        }

        [HttpGet]
        [Route("valida/tokens")]
        [AllowAnonymous]
        public IActionResult Get2()
        {
            var authenticationHeaders = Request.Headers["Authorization"].ToArray();
            if ((authenticationHeaders == null) || (authenticationHeaders.Length != 1))
            {
                return BadRequest();
            }
            var jwToken = authenticationHeaders[0].Split(' ')[1];
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken securityToken = null;
            try
            {
                principal = jwtSecurityTokenHandler.ValidateToken(jwToken, TokenBuilder.tokenValidationParams, out securityToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if ((principal != null) && (principal.Claims != null))
            {
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                Trace.WriteLine(jwtSecurityToken.Audiences.First());
                Trace.WriteLine(jwtSecurityToken.Issuer);
            }
            return Ok();
        }
    }
}