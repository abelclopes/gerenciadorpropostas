using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class PropostaHistorico : EntidadeBase
    {
        public PropostaHistorico()
        {}
        public PropostaHistorico(Proposta proposta, Usuario usuario)
        {
            PropostaId = proposta.Id;
            UsuarioId = usuario.Id;
            PropostaStatus = proposta.Status;
        }
        public Guid PropostaId { get; set; }

        public Proposta Propostas { get; set; }

        public virtual PropostaStatus PropostaStatus{ get; set; }
        public Guid UsuarioId { get; set; }
                
    }
}