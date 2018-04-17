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
        public Fornecedor(string nome, string cnpjCpf, string telefone)
        {
            Nome = nome;
            CnpjCpf = cnpjCpf;
            Telefone = telefone;
        }

        public string Nome { get; set; }
        public string CnpjCpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public async Task Atualizar(Fornecedor model, IContext _context)
        {
            if (await _context.Fornecedores.AnyAsync(x => x.Nome.Equals(model.Nome)))
                throw new ArgumentException($"O Nome {model.Nome} já esta em uso");
            if (await _context.Fornecedores.AnyAsync(x => x.Email.Equals(Email)))
                throw new ArgumentException($"O E-mail {model.Email} já esta em uso");            
            if (await _context.Fornecedores.AnyAsync(x => x.Email.Equals(Email)))
                throw new ArgumentException($"O Cnpj/Cpf {model.CnpjCpf} já esta em uso");
            
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
