


namespace Model
{
    public class PaginationParams{
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string buscaTermo { get; set; }
    }
    public class PaginationParamsProposta{
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string BuscaTermo { get; set; }
        public string NomeProposta { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string FornecedorID { get; set; }
        public string FornecedorNome { get; set; }
        public string CategoriaID { get; set; } 
        public string CategoriaNome { get; set; }
        public int? Status { get; set; }
        public string SortBy { get; set; }
        public string SortDir { get; set; }
 
    }
}
