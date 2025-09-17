using Confluent.Kafka;
using System.Text.Json;

namespace Ailos.ContaCorrente.API.Services;

public class KafkaService
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaService> _logger;

    public KafkaService(IConfiguration configuration, ILogger<KafkaService> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092"
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, string key, T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<string, string>
            {
                Key = key,
                Value = json
            };

            var result = await _producer.ProduceAsync(topic, kafkaMessage);
            _logger.LogInformation("Mensagem enviada para Kafka: {Topic} - {Key}", topic, key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar mensagem para Kafka: {Topic} - {Key}", topic, key);
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}
