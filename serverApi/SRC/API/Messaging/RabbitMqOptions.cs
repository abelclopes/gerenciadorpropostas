namespace API.Messaging
{
    public class RabbitMqOptions
    {
        public bool Enabled { get; set; } = true;
        public string HostName { get; set; } = "rabbitmq";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Exchange { get; set; } = "gerenciadorpropostas.events";
    }
}
