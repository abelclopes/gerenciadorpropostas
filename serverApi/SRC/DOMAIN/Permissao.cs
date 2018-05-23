using System;
using System.Collections.Generic;

namespace DOMAIN
{
    public class Permissao: EntidadeBase
    {
        public Permissao()
        {
        }

        public int Nivel { get; set; }
        public string Nome { get; set; }

        public ICollection<UsuarioPermissao> UsuarioPermissoes { get; set; }
    }
}
