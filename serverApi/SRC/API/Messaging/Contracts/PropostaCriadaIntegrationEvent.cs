using System;

namespace API.Messaging.Contracts
{
    public class PropostaCriadaIntegrationEvent
    {
        public Guid PropostaId { get; set; }
        public string NomeProposta { get; set; }
        public string Valor { get; set; }
        public int Status { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CategoriaId { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime CriadoEmUtc { get; set; }
    }
}
