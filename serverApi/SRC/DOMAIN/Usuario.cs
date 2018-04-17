using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Usuario : EntidadeBase
    {
         public Usuario(){}
        public Usuario(string nome)
        {
            Nome = nome;
        }
        public Usuario(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string telefone { get; set; }

        public async Task Atualizar(string nome, string email, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.Equals(nome)))
                throw new ArgumentException($"O Nome {nome} já esta em uso");
            if (await _context.Usuarios.AnyAsync(x => x.Email.Equals(Email)))
                throw new ArgumentException($"O E-mail {Email} já esta em uso");
            
            Nome = nome;
            Email = email;
        }
        
        public async Task Atualizar(string nome, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.Equals(nome)))
                throw new ArgumentException($"O Nome {nome} já esta em uso");
            
            Nome = nome;
        }
    }
}
