using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Categoria : EntidadeBase
    {
        public Guid CategoriaID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}