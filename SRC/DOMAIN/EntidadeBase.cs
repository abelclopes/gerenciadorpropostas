using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DOMAIN
{
    public class EntidadeBase
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Excluido { get; set; }

        public EntidadeBase()
        {
            DataCriacao = DateTime.UtcNow;
        }

        public void Excluir()
        {
            Excluido = true;
        }
    }
}