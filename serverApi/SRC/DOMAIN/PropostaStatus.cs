using System;
using System.ComponentModel;
using DOMAIN.Interfaces;

namespace DOMAIN
{
    public enum PropostaStatus
    {  
        [Description("Aguardando Avaliação")]
        AguardandoAvaliacao = 1, 
        [Description("Aprovado")]
        aprovado = 2,
        [Description("Expirado")]
        expirado = 3,
        [Description("Aguardando Avaliação do Analista Financeiro")]
        AguardandoAvaliacaoAnalistaFinanceiro = 4
    }
}