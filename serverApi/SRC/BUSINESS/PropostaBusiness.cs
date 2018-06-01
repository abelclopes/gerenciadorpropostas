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
        public IContext Context { get; }

        public PropostaSituacao situacao;
        private Proposta Proposta { get; set; }        
        private Usuario UsuarioAuthenticado { get; set; }
        private string VALOR_MAXIMO = "10000,00";

        public PropostaBusiness(IContext context)
        {   
            Context = context;
            this.situacao = new PropostaSituacao();
        }
        public PropostaBusiness()
        {   
            this.situacao = new PropostaSituacao();
        }        
        public PropostaSituacao validate(Proposta model, Usuario usuarioLogado, PropostaStatus status)
        { 
            this.UsuarioAuthenticado = usuarioLogado;
            var propostaHistorico = Context.PropostasHistoricos.Where(x => x.PropostaId == model.Id).ToList();            
            var permissao = Context.Permissoes.FirstOrDefault(x => x.Nivel.Equals(4));
            var usuarioPermissoes = Context.UsuarioPermissoes.Where(x => x.PermissaoId == permissao.Id).ToList();
            var usuarios = Context.Usuarios.Where(x => x.UsuarioPermissoes.PermissaoId == permissao.Id).ToList();


           this.situacao.NecessitaAprovavaoDiretorFinanceiro = propostaValor(model.Valor);
            if(model.Status != (PropostaStatus)3 || model.Status != (PropostaStatus)1)
            {   
               this.situacao.AprovadaDiretorFinanceiro = validaJaAporvado(model, propostaHistorico, usuarioPermissoes, usuarios, true);
               this.situacao.AprovadaAnalistaFinanceiro = validaJaAporvado(model, propostaHistorico, usuarioPermissoes, usuarios);
               this.situacao.Status = status;
                return this.situacao ;
            }            
            return this.situacao ;
        }
        private bool propostaValor(string valor)
        {
            valor = valor.Replace(".", ",");
            double numero = Convert.ToDouble(valor);
            if(numero > Convert.ToDouble(VALOR_MAXIMO)) return this.situacao.ValorPropostaAcimaDoLimiteDesMill = true;
            return false;
        }
        private bool validaJaAporvado(
            Proposta model, 
            List<PropostaHistorico> propostaHistorico,
            List<UsuarioPermissao> usuarioPermissoes, 
            List<Usuario> usuarios, 
            bool diretor = false
        ){
            propostaHistorico.Any( x => x.PropostaId == model.Id);
            var ph = propostaHistorico.Where( x => x.PropostaId == model.Id );
            var decisao = false;
            PropostaHistorico historico;
            if(ph.Any(x => x.PropostaStatus == (PropostaStatus)4) && diretor)
            {
                historico = propostaHistorico.FirstOrDefault(x => x.PropostaStatus == (PropostaStatus)4);
            }
            else
            {
                historico = propostaHistorico.FirstOrDefault(x => x.PropostaStatus == (PropostaStatus)1);
            }
//@todo
            switch((int)historico.PropostaStatus){
                case 4: // Aprovado por Diretor Financeiro
                    if(this.situacao.NecessitaAprovavaoDiretorFinanceiro  && diretor)
                    {
                        return decisao = true;
                    }
                break;
                case 1: // aprovar Analista Financeiro
                    if(!this.situacao.NecessitaAprovavaoDiretorFinanceiro  && diretor == false)
                    {
                        decisao = true;
                    }
                break;
                default:
                    this.situacao.AbilitaOpcaoAprovarProposta = false;
                    this.situacao.Aprovar = false;
                    return false;
            }
            if(decisao == true){
                this.situacao.AbilitaOpcaoAprovarProposta = true;
                this.situacao.Aprovar = true;
            }
            return decisao;
        }

        public bool validaSePropsotaExpirou(Proposta model)
        {
            DateTime dt = Convert.ToDateTime(model.DataCriacao);
            var ndt = dt;
            ndt = ndt.AddHours(24);
            DateTime dataAtual = Convert.ToDateTime(DateTime.Now);
            if(dataAtual.TimeOfDay.Ticks > ndt.TimeOfDay.Ticks)
            {
                return true;                
            }
            return false;
        }
    }
}