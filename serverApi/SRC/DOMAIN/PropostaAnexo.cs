using System;
using System.ComponentModel.DataAnnotations;
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
        public PropostaAnexo(byte[] nome, string _fileName, string _contentType)
        {
            this.Nome = nome;
            this.fileContent = _fileContent;
            this.contentType = _contentType;
        }

        public string Nome { get; set; }
        public string ContentType { get ; set;}
        public byte[] FileContent { get ; set;}
        public virtual Proposta Proposta { get ; set;}
        public void Atualizar(PropostaAnexo model, Proposta proposta)
        {
            this.Nome = model.Nome;
            this.ContentType = model.ContentType;
            this.FileContent = model.FileContent;
            this.Proposta = proposta;
        }
    }
}