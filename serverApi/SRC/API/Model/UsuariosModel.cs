using System;
using System.Text.RegularExpressions;
using DOMAIN;
using Model;

namespace Model
{
    public class UsuariosModel : EntidadeBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public UsuarioPermissao UsuarioPermissao { get; set; }
        public string Permissao { get; set; }
        public int PermissaoNivel { get; set; }
        public DateTime DataNacimento { get; set; }

       	private static DateTime convertDateTime(string date){
            string input = date;   
            string pattern = @"(-)|(/)";
            var datan = Regex.Split(input, pattern);
            
            return new DateTime(int.Parse(datan[4]), int.Parse(datan[2]),int.Parse(datan[0]));
            
        }
    }
}