using System;
using DOMAIN;
using Model;

namespace Model
{
    public class UsuariosModel : EntidadeBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public Perfil Perfil { get; set; }
        public DateTime DataNacimento { get; set; }
    }
}