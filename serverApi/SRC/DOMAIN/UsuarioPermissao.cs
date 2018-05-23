using System;
using System.Collections.Generic;

namespace DOMAIN
{
    public partial class UsuarioPermissao: EntidadeBase
    {
        public UsuarioPermissao()
        {
            
        }
        public UsuarioPermissao(Usuario usuario, Permissao permissoes )
        {
            UsuarioId = usuario.Id;
            PermissaoId = permissoes.Id;
            
        }
        public Guid UsuarioId { get; set; }
        public Guid PermissaoId { get; set; }

        public Permissao Permissoes { get; set; }
        public Usuario Usuario { get; set; }
    }
}
