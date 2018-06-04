using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BUSINESS.Model;
using DOMAIN;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BUSINESS
{
    public class PropostaBusiness
    {
        public IContext Context { get; }
        private Proposta Proposta { get; set; }        
        public PropostaSituacao situacao;
        private Usuario UsuarioAuthenticado { get; set; }
        private string VALOR_MAXIMO = "10000,00"; //verificar

        private int ADMINISTRADOR = 1;
        private int ANALISTA_COMPRAS = 2;
        private int ANALISTA_FINANCEIRO = 3;
        private int DIRETOR_FINANCEIRO = 4;
        
        private bool PropostaValida;

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
            var permissao = Context.Permissoes.FirstOrDefault(x => x.Nivel.Equals(4));
            var usuarioPermissoes = Context.UsuarioPermissoes.Where(x => x.PermissaoId == permissao.Id).ToList();


           this.situacao.NecessitaAprovavaoDiretorFinanceiro = propostaValor(model.Valor);
           
           // proposta Aprovada ou expirada nao executa validação
            if(model.Status != (PropostaStatus)3 || model.Status != (PropostaStatus)1) 
            {
                this.situacao.Status = GetPropostaValida(model.Id).Status;
                return validaJaAporvado(model);
            }            
            return this.situacao ;
        }
        private bool propostaValor(string valor)
        {
            valor = valor.Replace(".", ",");
            double numero = Convert.ToDouble(valor);
            if(numero == Convert.ToDouble(VALOR_MAXIMO)) return this.situacao.ValorPropostaAcimaDoLimiteDesMill = false;
            if(numero > Convert.ToDouble(VALOR_MAXIMO)) return this.situacao.ValorPropostaAcimaDoLimiteDesMill = true;
            return false;
        }
        private PropostaSituacao validaJaAporvado(Proposta model)
        {
            var ph = Context.PropostasHistoricos.Where(x => x.PropostaId == model.Id).ToList();
            var UsuarioPermissao = getPermissaoUsuarioLogado();
            // se for analista financeiro pode aprovar
            if(ph.Any(x => x.PropostaStatus == (PropostaStatus)4) && !ph.Any(x => x.PropostaStatus == (PropostaStatus)2) 
                && (this.situacao.Status == (PropostaStatus)4) && (this.situacao.Status != (PropostaStatus)2)
                && this.situacao.NecessitaAprovavaoDiretorFinanceiro && UsuarioPermissao.Nivel.Equals(DIRETOR_FINANCEIRO))
            {
                this.situacao.AprovadaDiretorFinanceiro = true;                
                this.situacao.AprovadaAnalistaFinanceiro = false;
                this.situacao.Aprovar = true;
                this.situacao.AbilitaOpcaoAprovarProposta = true;
            }
            // somente diretor financeiro pode aprovar
            else if(!ph.Any(x => x.PropostaStatus == (PropostaStatus)4) && !ph.Any(x => x.PropostaStatus == (PropostaStatus)2)
                && (this.situacao.Status != (PropostaStatus)4) && (this.situacao.Status != (PropostaStatus)2)
                && this.situacao.NecessitaAprovavaoDiretorFinanceiro && UsuarioPermissao.Nivel.Equals(DIRETOR_FINANCEIRO))
            {                
                this.situacao.AprovadaDiretorFinanceiro = true;
                this.situacao.AprovadaAnalistaFinanceiro = true;
                this.situacao.Aprovar = true;
                this.situacao.AbilitaOpcaoAprovarProposta = true;
            }
            // nao necessita de diretor financeiro para aprovar
            else if(!ph.Any(x => x.PropostaStatus == (PropostaStatus)4) && !ph.Any(x => x.PropostaStatus == (PropostaStatus)2)  
                && (this.situacao.Status != (PropostaStatus)4) && (this.situacao.Status != (PropostaStatus)2)
                && !this.situacao.NecessitaAprovavaoDiretorFinanceiro && UsuarioPermissao.Nivel.Equals(ANALISTA_FINANCEIRO))
            {
                this.situacao.Aprovar = true;
                this.situacao.AbilitaOpcaoAprovarProposta = true;
            }
            // administardor nao pode aprovar
            else if(!ph.Any(x => x.PropostaStatus == (PropostaStatus)4) && !ph.Any(x => x.PropostaStatus == (PropostaStatus)2)  
                && (this.situacao.Status != (PropostaStatus)4) && (this.situacao.Status != (PropostaStatus)2)
                && !this.situacao.NecessitaAprovavaoDiretorFinanceiro && UsuarioPermissao.Nivel.Equals(ADMINISTRADOR) || UsuarioPermissao.Nivel.Equals(ANALISTA_COMPRAS))
            {
                this.situacao.Aprovar = false;
                this.situacao.AbilitaOpcaoAprovarProposta = false;
            }
            return this.situacao;
        }

        public bool propostaIsValid(Guid Id)
        {
            GetPropostaValida(Id);
            return PropostaValida;
        }
        public Proposta GetPropostaValida(Guid Id)
        {
            Proposta proposta = Context.Propostas.FirstOrDefault(x => x.Id == Id  && x.Status != (PropostaStatus)3 || x.Status != (PropostaStatus)1 && !x.Excluido);
            PropostaValida = (proposta.NomeProposta != null)? true: false;
            return proposta;
        }

        public Permissao getPermissaoUsuarioLogado()
        {
            var permissaoId = Context.UsuarioPermissoes.FirstOrDefault(x => x.Usuario.Id == this.UsuarioAuthenticado.Id);
            return Context.Permissoes.FirstOrDefault(x => x.Id == permissaoId.PermissaoId);
        }
        public async Task<Usuario> getUsuarioLogado(string id){
            this.UsuarioAuthenticado = await Context.Usuarios.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return this.UsuarioAuthenticado;
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