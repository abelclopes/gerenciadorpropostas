using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Fornecedor : EntidadeBase
    {
        public Fornecedor(){}
        public Fornecedor(string nome)
        {
            Nome = nome;
        }
        public Fornecedor(string nome, string email, string cnpjCpf, string telefone)
        {
            Nome = nome;
            Email = email;
            CnpjCpf = cnpjCpf;
            Telefone = telefone;
        }

        public string Nome { get; set; }
        public string CnpjCpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public void Atualizar(Fornecedor model, IContext _context)
        {
            Nome = model.Nome;
            CnpjCpf = model.CnpjCpf;
            Telefone = model.Telefone;
            Email = model.Email;
        }
        
        public async Task Atualizar(string nome, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.Equals(nome)))
                throw new ArgumentException($"O Nome {nome} já esta em uso");
            
            Nome = nome;
        }
    }
}
