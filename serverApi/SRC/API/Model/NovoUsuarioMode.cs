using System;
using DOMAIN;
using Model;

namespace Model
{
    public class NovoUsuarioModel  
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataNacimento { get; set; }
        public int Perfil { get; set; }
        public Guid PermissaoId { get; set; }

    }
}