using System;
using System.ComponentModel.DataAnnotations;
using DOMAIN;
using Model;

namespace Model
{
    public class PropostaPaginationModel: EntidadeBase
    {
        public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public virtual PropostaHistorico PropostaHistorico { get; set; }
        public PropostaStatus Status { get; set; }
        public Guid CategoriaId { get; internal set; }
        public string CategoriaNome { get; set; } 
        public Categoria Categoria { get; set; } 
        public Guid FornecedorId { get; internal set; }
        public string FornecedorNome { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}