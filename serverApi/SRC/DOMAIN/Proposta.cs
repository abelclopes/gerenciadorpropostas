using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Proposta : EntidadeBase
    {
        public Proposta()
        {
        }
        public Proposta(string nomeProposta, string descricao, double valor, Fornecedor fornecedor, Categoria categoria)
        {
            NomeProposta = nomeProposta;
            Descricao = descricao;
            Fornecedor = fornecedor;
            Valor = valor;
            Categoria = categoria;
        }

        public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Categoria Categoria { get; set; }

        public void Atualizar(Proposta model, IContext context)
        {
            
        }
    }
}