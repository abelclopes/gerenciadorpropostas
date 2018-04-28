using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class PropostaHistorico : EntidadeBase
    {
        public Proposta Proposta { get; set; }

        public virtual PropostaStatus PropostaStatus{ get; set; }

        public virtual Usuario user { get; set; }
                
    }
}