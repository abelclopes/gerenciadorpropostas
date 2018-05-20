using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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
        public string Usuario { get; set; }
        public IFormFile Anexo { get; set; }
    }
}