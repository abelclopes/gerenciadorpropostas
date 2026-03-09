using System;

namespace Model
{
    public class DashboardKpiModel
    {
        public int TotalPropostas { get; set; }
        public int PropostasAguardando { get; set; }
        public int PropostasAprovadas { get; set; }
        public int PropostasOutrosStatus { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}
