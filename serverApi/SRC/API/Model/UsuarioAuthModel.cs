using System;
using DOMAIN;
using Model;

namespace Model
{
    public class UsuarioAuthModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Police { get; set; }
        public DateTime DataNacimento { get; set; }
        public int Idade { get; set; }
        public bool Excluido  { get; set; }
    }
}