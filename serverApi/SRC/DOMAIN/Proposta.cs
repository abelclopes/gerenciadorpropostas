using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Proposta : EntidadeBase
    {
        public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; }

        [DataType(DataType.DateTime)]
        public double valor { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public Categoria Categoria { get; set; }        
                
    }
}