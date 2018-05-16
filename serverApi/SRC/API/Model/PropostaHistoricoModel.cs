using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DOMAIN;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class PropostaHistoricoModel : BaseModel
    {
        public Guid PropostaId { get; set; }

        public Proposta Proposta { get; set; }

        public virtual PropostaStatus PropostaStatus{ get; set; }
        public Guid UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }
                
    }
}