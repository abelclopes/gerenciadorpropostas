using System;
using System.ComponentModel;
using DOMAIN.Interfaces;

namespace DOMAIN
{
    public enum PropostaStatus
    {  
        [Description("Aguardando Avaliação")]
        PROPOSTA_AGUARDANDO_APROVACAO = 1, 
        [Description("Aprovado")]
        PROPOSTA_APROVADA_FINANCEIRO = 2,
        PROPOSTA_APROVADA= 2,
        [Description("Expirado")]
        PROPOSTA_EXPIRADA = 3,
        [Description("Aguardando Avaliação do Analista Financeiro")]
        PROPOSTA_APROVADA_DIRETOR = 4,
        [Description("Rejeitada")]
        PROPOSTA_REJEITADA = 5
    }
}