using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace webhookSben.KafkaProducer
{
    public class KafkaProducerService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly string _saslUsername;
        private readonly string _saslPassword;

        public KafkaProducerService(IConfiguration config)
        {
            _bootstrapServers = config["Kafka:BootstrapServers"];
            _topic = config["Kafka:Topic"];
            _saslUsername = config["Kafka:SaslUsername"];
            _saslPassword = config["Kafka:SaslPassword"];
        }

        public async Task ProduceAsync<T>(T message)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = _bootstrapServers,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.Plain,
                    SaslUsername = _saslUsername,
                    SaslPassword = _saslPassword
                };

                using var producer = new ProducerBuilder<Null, string>(config).Build();

                string jsonMessage = JsonSerializer.Serialize(message);
                Console.WriteLine($"Produciendo mensaje JSON: {jsonMessage}");

                var result = await producer.ProduceAsync(
                    _topic,
                    new Message<Null, string> { Value = jsonMessage }
                );

                Console.WriteLine($"Mensaje enviado a Kafka con offset: {result.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Error en Kafka ProduceException: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al enviar mensaje a Kafka: {ex.Message}");
            }
        }
    }
}
