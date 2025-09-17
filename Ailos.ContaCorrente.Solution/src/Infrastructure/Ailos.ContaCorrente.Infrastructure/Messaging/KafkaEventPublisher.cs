using Ailos.Shared.Common.Interfaces;
using Confluent.Kafka;
using System.Text.Json;

namespace Ailos.ContaCorrente.Infrastructure.Messaging;

public class KafkaEventPublisher : IEventPublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaEventPublisher(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, T eventData) where T : class
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(eventData)
        };

        await _producer.ProduceAsync(topic, message);
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}
