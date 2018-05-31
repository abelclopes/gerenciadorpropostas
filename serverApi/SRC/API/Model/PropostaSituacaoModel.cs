using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DOMAIN;
using Model;

namespace Model
{
    public class PropostaSituacaoModel
    {
        public virtual ICollection<PropostaHistorico> PropostaHistorico { get; set; }
        public PropostaStatus Status { get; set; }
        public Boolean Aprovar { get; set; }
        public Boolean ValorPropostaAcimaDoLimiteDesMill { get; set; }
        public Boolean NecessitaAprovavaoDiretorFinanceiro { get; set; }
        public Boolean AprovavaoDiretorFinanceiro { get; set; }
        public Boolean AprovadaAnalistaFinanceiro { get; set; }
     
    }
}