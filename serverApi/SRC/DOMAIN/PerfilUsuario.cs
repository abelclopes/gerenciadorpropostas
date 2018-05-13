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
        [Description("Analista De Compras")]
        AnalistaDeCompras = 2,
        [Description("Analista Financeiro")]
        AnalistaFinanceiro = 3,
        [Description("Diretor Financeiro")]
        DiretorFinanceiro = 4
    }
}