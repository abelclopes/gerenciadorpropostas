using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Proposta : EntidadeBase
    {
        public Proposta()
        {
            PropostaAnexos = new HashSet<PropostaAnexo>();
            PropostaHistoricos = new HashSet<PropostaHistorico>();
        }
        public Proposta(string nomeProposta, string descricao, string valor, Fornecedor fornecedor, Categoria categoria, PropostaStatus status)
        {
            NomeProposta = nomeProposta;
            Descricao = descricao;
            Fornecedor = fornecedor;
            FornecedorId = fornecedor.Id;            
            Valor = valor;
            Categoria = categoria;
            CategoriaId = categoria.Id;
            Status = status;
        }

        public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public PropostaStatus Status { get; set; }
        [ForeignKey("Categoria")]
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        [ForeignKey("Fornecedor")]
        public Guid FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public ICollection<PropostaHistorico> PropostaHistoricos { get; set; }

        public ICollection<PropostaAnexo> PropostaAnexos { get; set; }



        public void Atualizar(Proposta model)
        {
            NomeProposta = model.NomeProposta;
            Descricao = model.Descricao;
            Fornecedor = model.Fornecedor;
            FornecedorId = model.Fornecedor.Id;
            Valor = model.Valor;
            Categoria = model.Categoria;
            CategoriaId = model.Categoria.Id;
            Status = model.Status;
        }
    }
}