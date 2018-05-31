using System;
using System.Collections.Generic;
using System.Linq;
using BUSINESS.Model;
using DOMAIN;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BUSINESS
{
    public class PropostaBusiness
    {
        
        private PropostaSituacao situacao;
        private double VALOR_MAXIMO = 10000.00;

        private PropostaStatusBussness ListStatus;
        private IContext Context;

        public PropostaBusiness()
        {
        }

        public PropostaBusiness(IContext context)
        {
            Context = context;
            situacao = new PropostaSituacao();
            ListStatus = new PropostaStatusBussness();
        }
        
        private Proposta Proposta{ get; set; }

        public PropostaSituacao validate(Proposta model, int status)
        {
            this.situacao.NecessitaAprovavaoDiretorFinanceiro = propostaValor(model.Valor);            

            if(Context.PropostasHistoricos.Any( x => x.PropostaStatus.Equals(model.Status)))
            {   
                validaJaAporvadoAnalistaFinanceiro(model);
                this.situacao.Status =  ListStatus.ListStatus.FirstOrDefault(x =>x.Id == status).Id;
                return situacao;
            }            
            return situacao;
        }

        private Boolean propostaValor(double valor)
        {
            if(valor > VALOR_MAXIMO) return true;
            return false;
        }

        private Boolean validaJaAporvadoAnalistaFinanceiro(Proposta model)
        {
            var ph = Context.PropostasHistoricos.Where( x => x.PropostaId == model.Id);
            foreach(var historico in ph)
            {
                var userPermissao = Context.UsuarioPermissoes.FirstOrDefault(x => x.UsuarioId == historico.UsuarioId);
                if(this.situacao.NecessitaAprovavaoDiretorFinanceiro)
                {
                    if(Context.Permissoes.Any(x=> x.Nivel.Equals(4) && x.Id == userPermissao.PermissaoId)){
                        return this.situacao.AprovavaoDiretorFinanceiro = true;
                    }
                }
            }
            return false;
        }

        public bool validaSePropsotaExpirou(Proposta model,IContext Context)
        {
            DateTime dt = Convert.ToDateTime(model.DataCriacao); 
            DateTime dataAtual = DateTime.Now;
            if(dataAtual > dt) return true;

            return false;
        }
    }
}