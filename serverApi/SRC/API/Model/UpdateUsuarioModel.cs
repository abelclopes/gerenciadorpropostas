using System;
using DOMAIN;
using Model;

namespace Model
{
    public class UpdateUsuarioModel : BaseDataModel
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string DataNacimento { get; set; }
        public int perfilUsuario { get; set; }
        public Guid PermissaoId { get; set; }

    }
}