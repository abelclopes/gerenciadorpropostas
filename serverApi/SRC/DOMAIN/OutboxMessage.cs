using System;

namespace DOMAIN
{
    public class OutboxMessage : EntidadeBase
    {
        public string Type { get; set; }
        public string RoutingKey { get; set; }
        public string Payload { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int Attempts { get; set; }
        public string LastError { get; set; }
    }
}
