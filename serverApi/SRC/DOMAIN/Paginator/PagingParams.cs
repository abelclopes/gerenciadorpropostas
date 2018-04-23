using System;
using System.ComponentModel;
using System.Reflection;

namespace DOMAIN.Paginator 
{
    public class PagingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }  
}