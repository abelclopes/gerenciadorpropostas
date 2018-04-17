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
        public Usuario(string nome, string cpf, DateTime dataNacimento, Perfil perfil)
        {
            Nome = nome;
            Cpf = cpf;
            DataNacimento = dataNacimento;
            PerfilUsuario = perfil;
        }

        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public DateTime DataNacimento { get; set; }
        public Perfil PerfilUsuario { get; set; }

        public async Task Atualizar(Usuario model, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.Equals(model.Nome)))
                throw new ArgumentException($"O Nome {model.Nome} já esta em uso");
            if (await _context.Usuarios.AnyAsync(x => x.Email.Equals(Email)))
                throw new ArgumentException($"O E-mail {model.Email} já esta em uso");
            
            Nome = model.Nome;
            Cpf = model.Cpf;
            DataNacimento = model.DataNacimento;
            PerfilUsuario = model.PerfilUsuario;
        }
        
        public async Task Atualizar(string nome, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.Equals(nome)))
                throw new ArgumentException($"O Nome {nome} já esta em uso");
            
            Nome = nome;
        }
    }
}
