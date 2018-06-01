using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DOMAIN;

namespace BUSINESS.Model
{
    /*
        RN05.01 - Aprovação das propostas: para uma proposta possuir o status de
        “Aprovada”, a mesma deve possuir a aprovação, no mínimo, do analista financeiro.
        RN05.02 - Propostas de alto valor: caso as propostas possuam valores superiores a
        R$10.000,00, a mesma precisa também da aprovação do Diretor Financeiro.
        RN05.03 - Status possíveis para as propostas: as propostas podem assumir os
        status de “Aprovada”, “Reprovada”, “Pendente Diretoria”, sendo o último status,
        exibido apenas quando à proposta ultrapassar o valor estabelecido pela RN05.02.
        Sendo assim, à mesma precisará também da aprovação do diretor financeiro para
        assumir o status de “Aprovada”.
    */
    public class PropostaSituacao
    {
        public ICollection<PropostaHistorico> PropostaHistorico { get; set; }
        
        /// <summary>
        /// Status
        /// Description A ( RN05.03 - Status possíveis para as propostas: as propostas podem assumir os
        /// status de “Aprovada”, “Reprovada”, “Pendente Diretoria”, sendo o último status,
        /// exibido apenas quando à proposta ultrapassar o valor estabelecido pela RN05.02.
        /// Sendo assim, à mesma precisará também da aprovação do diretor financeiro para
        /// assumir o status de “Aprovada”.)
        /// </summary>
        public PropostaStatus Status { get; set; }
        public bool Aprovar { get; set; }
        /// <summary>
        /// ValorPropostaAcimaDoLimiteDesMill
        /// Description A ( RN05.02 - Propostas de alto valor: caso as propostas possuam valores superiores a R$10.000,00, 
        /// a mesma precisa também da aprovação do Diretor Financeiro.)
        /// </summary>
        public bool ValorPropostaAcimaDoLimiteDesMill { get; set; }
        
        /// <summary>
        /// NecessitaAprovavaoDiretorFinanceiro
        /// Description ( Leia a RN05.02.)
        /// </summary>
        public bool NecessitaAprovavaoDiretorFinanceiro { get; set; }
        
        /// <summary>
        /// AprovavaoDiretorFinanceiro
        /// Description A ( Leia a RN05.02.)
        /// </summary>
        public bool AprovadaDiretorFinanceiro { get; set; }
         
        /// <summary>
        /// AprovadaAnalistaFinanceiro
        /// Description (RN05.01 - Aprovação das propostas: para uma proposta possuir o status de
        /// “Aprovada”, a mesma deve possuir a aprovação, no mínimo, do analista financeiro.)
        /// </summary>
        public bool AprovadaAnalistaFinanceiro { get; set; }
        /// <summary>
        /// AbilitaOpcaoAprovarProposta
        /// Description (RN05.01 - Aprovação das propostas: para uma proposta possuir o status de
        /// “Aprovada”, a mesma deve possuir a aprovação, no mínimo, do analista financeiro.)
        /// </summary>
        public bool AbilitaOpcaoAprovarProposta { get; set; }
     
    }
}