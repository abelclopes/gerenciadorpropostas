using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DOMAIN
{
    public class PermissaoUsuario : EntidadeBase
    {
        public PermissaoUsuario() {}
        public PermissaoUsuario(string permissao, int nivel) 
        { 
            Permissao = permissao;
            Nivel = nivel;
        }
      public string Permissao { get; set; }
      
      public int Nivel { get; set; }
    }
}