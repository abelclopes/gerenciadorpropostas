using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class PropostaAnexo : EntidadeBase
    {
        public PropostaAnexo()
        {
        }

        public PropostaAnexo(byte[] fileContent, string fileName, string contentType, Proposta proposta)
        {
            Nome = fileName;
            FileContent = fileContent;
            ContentType = contentType;
            Proposta = proposta;
        }

        public string Nome { get; set; }
        public string ContentType { get ; set;}
        public byte[] FileContent { get ; set;}
        public Proposta Proposta { get; set; }
        public void Add(PropostaAnexo model, Proposta proposta)
        {
            this.Nome = model.Nome;
            this.ContentType = model.ContentType;
            this.FileContent = model.FileContent;
            this.Proposta = proposta;
        }
    }
}