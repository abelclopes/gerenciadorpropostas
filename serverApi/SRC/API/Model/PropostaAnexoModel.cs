using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DOMAIN;
using Model;

namespace Model
{
    public class PropostaAnexoModel: EntidadeBase
    {
        public string Nome { get; set; }
        public string ContentType { get ; set;}
        public byte[] FileContent { get ; set;}
        public Proposta Proposta { get; set; }
    }
}