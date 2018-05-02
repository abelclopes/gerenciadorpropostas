using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class ParametrosPaginacao
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int NumeroPagina { get; set; }
        [Required]
        [Range(10, 100)]
        public int TamanhoPagina { get; set; }
    }

}