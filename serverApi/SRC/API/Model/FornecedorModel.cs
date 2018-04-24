using System;
using DOMAIN;
using Model;

namespace Model
{
    public class FornecedorModel: EntidadeBase
    {
        public string Nome { get; set; }
        public string CnpjCpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}