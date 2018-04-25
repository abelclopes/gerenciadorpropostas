


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
        public string NomeProposta { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string FornecedorID { get; set; }
        public string CategoriaID { get; set; }  
    }
}