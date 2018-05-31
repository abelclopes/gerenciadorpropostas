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
        public virtual ICollection<PropostaHistorico> PropostaHistorico { get; set; }
        
        /// <summary>
        /// Status
        /// Description A ( RN05.03 - Status possíveis para as propostas: as propostas podem assumir os
        /// status de “Aprovada”, “Reprovada”, “Pendente Diretoria”, sendo o último status,
        /// exibido apenas quando à proposta ultrapassar o valor estabelecido pela RN05.02.
        /// Sendo assim, à mesma precisará também da aprovação do diretor financeiro para
        /// assumir o status de “Aprovada”.)
        /// </summary>
        public int Status { get; set; }
        public Boolean Aprovar { get; set; }
        /// <summary>
        /// ValorPropostaAcimaDoLimiteDesMill
        /// Description A ( RN05.02 - Propostas de alto valor: caso as propostas possuam valores superiores a R$10.000,00, 
        /// a mesma precisa também da aprovação do Diretor Financeiro.)
        /// </summary>
        public Boolean ValorPropostaAcimaDoLimiteDesMill { get; set; }
        
        /// <summary>
        /// NecessitaAprovavaoDiretorFinanceiro
        /// Description ( Leia a RN05.02.)
        /// </summary>
        public Boolean NecessitaAprovavaoDiretorFinanceiro { get; set; }
        
        /// <summary>
        /// NecessitaAprovavaoDiretorFinanceiro
        /// Description A ( Leia a RN05.02.)
        /// </summary>
        public Boolean AprovavaoDiretorFinanceiro { get; set; }
         
        /// <summary>
        /// NecessitaAprovavaoDiretorFinanceiro
        /// Description (RN05.01 - Aprovação das propostas: para uma proposta possuir o status de
        /// “Aprovada”, a mesma deve possuir a aprovação, no mínimo, do analista financeiro.)
        /// </summary>
        public Boolean AprovadaAnalistaFinanceiro { get; set; }
     
    }
}