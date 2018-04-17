using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class Perfil : EntidadeBase
    {
        public Guid PerfilID { get; set; }
        public string Descricao { get; set; }
    }
}