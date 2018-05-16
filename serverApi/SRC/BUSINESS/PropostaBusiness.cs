using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN;


namespace BUSINESS
{
    public class PropostaBusiness
    {
        private ApplicationDbContext Context;
        public PropostaBusiness(IContext context){
            this.Context = context;
        }
        
        private Proposta Proposta{ get; set; }

        public void validate(Proposta model){
            if(mode.PorpostaHistorico.Any( x => x.PropostaStatus.aprovado)){
                return true
            }
            return false;
        }
    }
}