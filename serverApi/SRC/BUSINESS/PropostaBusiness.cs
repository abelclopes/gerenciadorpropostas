using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN;
using DOMAIN.Interfaces;

namespace BUSINESS
{
    public class PropostaBusiness
    {
        private IContext Context;
        public PropostaBusiness(IContext context)
        {
            this.Context = context;
        }
        
        private Proposta Proposta{ get; set; }

        public Boolean validate(Proposta model, int statusAtual)
        {

            if(model.PropostaHistorico.Any( x => x.PropostaStatus.Equals(2)))
            {
                return true;
            }            
            return false;
        }
    }
}