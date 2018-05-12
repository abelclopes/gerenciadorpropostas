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
        public Guid PropostaId { get; set; }

        public Proposta Proposta { get; set; }

        public virtual PropostaStatus PropostaStatus{ get; set; }
        public Guid UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }
                
    }
}