using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DOMAIN;
using Model;

namespace Model
{
    public class PropostaSituacaoModel
    { 
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public PropostaStatus Status { get; set; }
     
    }
}