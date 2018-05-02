using System;
using System.ComponentModel.DataAnnotations;
using DOMAIN;
using Model;

namespace Model
{
    public class NovaPropostaModel
    {
         public string NomeProposta { get; set; }
        
        [MaxLength(500)]
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string FornecedorID { get; set; }
        public string CategoriaID { get; set; }  
        public int Status { get; set; }
        public Byte[] Anexo { get; set; }
    }
}