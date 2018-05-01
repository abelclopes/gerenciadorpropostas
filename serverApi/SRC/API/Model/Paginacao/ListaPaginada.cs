using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Model
{
    public class ListaPaginada<T>
    {
        public ListaPaginada(int numeroPagina, int tamanhoPagina)
        {
            this.NumeroPagina = numeroPagina;
            this.TamanhoPagina = tamanhoPagina;
        }
        public int TotalItens { get; set; }
        public int NumeroPagina { get; }
        public int TamanhoPagina { get; }
        public List<T> Resultado { get; set; }
        public int TotalPaginas => (int)Math.Ceiling(this.TotalItens / (double)this.TamanhoPagina);
        public bool TemPaginaAnterior => this.NumeroPagina > 1;
        public bool TemPaginaPosterior => this.NumeroPagina < this.TotalPaginas;
        public int NumeroPaginaPosterior => this.TemPaginaPosterior ? this.NumeroPagina + 1 : this.TotalPaginas;
        public int NumeroPaginaAnterior => this.TemPaginaAnterior ? this.NumeroPagina - 1 : 1;

        public async Task<ListaPaginada<T>> Carregar(IQueryable<T> source)
        {
            this.TotalItens = await source.CountAsync();
            this.Resultado = await source.Skip(TamanhoPagina * (NumeroPagina - 1))
                                         .Take(TamanhoPagina)
                                         .ToListAsync();

            return this;
        }

        public ListaPaginada<T> Carregar(List<T> source)
        {
            this.TotalItens = source.Count();
            this.Resultado = source.Skip(TamanhoPagina * (NumeroPagina - 1))
                                   .Take(TamanhoPagina).ToList();                              

            return this;
        }
    }
}