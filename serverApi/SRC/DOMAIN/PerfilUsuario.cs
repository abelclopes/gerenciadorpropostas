using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public enum Perfil
    {
        
        [Description("Administrador")]
        Administrador = 1,
        [Description("AnalistaDeCompras")]
        AnalistaDeCompras = 2,
        [Description("AnalistaFinanceiro")]
        AnalistaFinanceiro = 3,
        [Description("DiretorFinanceiro")]
        DiretorFinanceiro = 4
    }
}