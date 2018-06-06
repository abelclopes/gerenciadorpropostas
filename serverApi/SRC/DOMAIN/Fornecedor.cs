using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Fornecedor : EntidadeBase
    {
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

        public Fornecedor()
        {
           // Propostas = new HashSet<Proposta>();
        }
        public string CnpjCpf { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }

        //public ICollection<Proposta> Propostas { get; set; }

        public void Atualizar(Fornecedor model, IContext _context)
        {
            Nome = model.Nome;
            CnpjCpf = model.CnpjCpf;
            Telefone = model.Telefone;
            Email = model.Email;
        }
    }
}
