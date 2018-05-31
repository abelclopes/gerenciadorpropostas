

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DOMAIN;

namespace BUSINESS.Model
{
    
    public class PropostaStatusBussness
    {  
        public PropostaStatusBussness(){
            ListStatus = getListStatus();
        }
        public int Id { get; set; }
        public string Status { get; set; }

        public List<PropostaStatusBussness> ListStatus { get; set; }
        
        private List<PropostaStatusBussness> getListStatus()
        {  
            return new List<PropostaStatusBussness>(){
                new PropostaStatusBussness{Status = "Aguardando Avaliação", Id = 1},
                new PropostaStatusBussness{Status = "Aprovado", Id = 2}, 
                new PropostaStatusBussness{Status = "Expirado", Id = 3}
            };
        }
    }
}
