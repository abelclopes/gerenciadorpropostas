using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public enum Perfil
    {
        Administrador,
        AnalistaDeCompras,
        AnalistaFinanceiro,
        DiretorFinanceiro
    }
}