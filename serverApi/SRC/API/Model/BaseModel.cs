using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public abstract class BaseModel
    {
        public override string ToString()
        {
            var values = GetType().GetProperties().Select(x => $"{x.Name} - {x.GetValue(this)}");

            return string.Join(", ", values);
        }
    }

    public abstract class BaseDataModel : BaseModel
    {
        public Guid Id { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }


}
