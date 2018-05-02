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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCriacao { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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