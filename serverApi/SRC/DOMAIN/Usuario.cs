﻿using System;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace DOMAIN
{
    public class Usuario : EntidadeBase
    {
        public Usuario(){}
        public Usuario(string nome)
        {
            Nome = nome;
        }
        public Usuario(string nome, string email,string cpf, DateTime dataNacimento, Perfil perfil)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            DataNacimento = dataNacimento;
            PerfilUsuario = perfil;
        }
        public Usuario(string nome, string email, string cpf, DateTime dataNacimento, Perfil perfil, string senha)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            DataNacimento = dataNacimento;
            PerfilUsuario = perfil;
            Senha = Util.GetSHA1HashData(senha);
        }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataNacimento { get; set; }
        public virtual Perfil PerfilUsuario { get; set; }

        public virtual PropostaHistorico PropostaHistoricos { get; set; }
        public void Atualizar(Usuario model, IContext _context)
        {
            Nome = model.Nome;
            Cpf = model.Cpf;
            Email = model.Email;
            DataNacimento = model.DataNacimento;
            PerfilUsuario = model.PerfilUsuario;
        }
        
        public async Task Atualizar(string nome, IContext _context)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Nome.Equals(nome)))
                throw new ArgumentException($"O Nome {nome} já esta em uso");
            
            Nome = nome;
        }
        public async Task AtualizarEmail(string email, IContext _context)
        {
            if (!await _context.Usuarios.AnyAsync(x => !x.Email.Equals(email)))
                throw new ArgumentException($"O e-mail {email} já está em uso!");
            
            Email = email;
        }
        public void AtualizarSenha(string senha, IContext _context)
        {
            Senha = Util.GetSHA1HashData(senha);
        }        
    }
}
