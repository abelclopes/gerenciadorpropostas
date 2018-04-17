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

        public PropostaStatus PropostaStatus{ get; set; }        
                
    }
}