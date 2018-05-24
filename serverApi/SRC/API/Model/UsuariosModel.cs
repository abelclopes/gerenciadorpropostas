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
        public UsuarioPermissao UsuarioPermissao { get; set; }
        public Permissoao Permissao { get; set; }
        public DateTime DataNacimento { get; set; }

       	private static DateTime convertDateTiem(string date){
            string input = date;   
            string pattern = @"(-)|(/)";
            var datan = Regex.Split(input, pattern);
            
            return new DateTime(int.Parse(datan[4]), int.Parse(datan[2]),int.Parse(datan[0]));
            
        }
    }
}