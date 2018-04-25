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
        public Proposta(string nomeProposta, string descricao, double valor, Fornecedor fornecedor, Categoria categoria, PropostaStatus status)
        {
            NomeProposta = nomeProposta;
            Descricao = descricao;
            Fornecedor = fornecedor;
            Valor = valor;
            Categoria = categoria;
            Status = status;
        }

        public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Categoria Categoria { get; set; }
        public PropostaStatus Status { get; set; }
        public void Atualizar(Proposta model, IContext context)
        {
            NomeProposta = model.NomeProposta;
            Descricao = model.Descricao;
            Fornecedor = model.Fornecedor;
            Valor = model.Valor;
            Categoria = model.Categoria;
            Status = model.Status;
        }
    }
}