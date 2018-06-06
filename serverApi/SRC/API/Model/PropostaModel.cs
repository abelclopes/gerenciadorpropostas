using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DOMAIN;
using Model;

namespace Model
{
    public class PropostaModel: EntidadeBase
    {
        public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; } 
        [DisplayFormat(DataFormatString = "{0:n2}",
            ApplyFormatInEditMode = true)]
        public string Valor { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual ICollection<PropostaHistorico> PropostaHistorico { get; set; }
        public virtual Categoria Categoria { get; set; }
        public PropostaStatus Status { get; set; }
        public Guid CategoriaId { get; internal set; }
        public Guid FornecedorId { get; internal set; }
    }
}