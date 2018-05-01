using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Categoria : EntidadeBase
    {
        public Categoria()
        {
        }
        public Categoria(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public void Atualizar(Categoria model, IContext _context)
        {
            Nome = model.Nome;
            Descricao = model.Descricao;
        }
        public void Atualizar(string nome, IContext _context)
        {
            Nome = nome;
        }
    }
}