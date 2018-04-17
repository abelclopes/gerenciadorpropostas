using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
        public string Senha { get {return Senha; } set{ Senha = SHA1Hash(Senha);} }
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
        public async Task Atualizar(string Email, string senha, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => !x.Email.Equals(Email)))
                throw new ArgumentException($"O e-mail {Email} não foi encontrado!");
            
            Senha = senha;
        }
        
        public string SHA1Hash(string input)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = sha.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
