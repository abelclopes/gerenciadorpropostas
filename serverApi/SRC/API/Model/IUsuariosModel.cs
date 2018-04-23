using System;
using DOMAIN;
using Model;

namespace Model
{
    public class IUsuariosModel: EntidadeBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public Perfil Police { get; set; }
        public DateTime DataNacimento { get; set; }

        public string Cpf { get; set; }
    }
}