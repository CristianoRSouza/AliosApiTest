using Confluent.Kafka;

namespace Ailos.Transferencia.API.Services;

public class MockKafkaProducer : IProducer<string, string>
{
    public Handle Handle => new Handle();
    public string Name => "MockProducer";

    public void Dispose() { }
    public int AddBrokers(string brokers) => 0;
    public void Flush(CancellationToken cancellationToken = default) { }
    public int Flush(TimeSpan timeout) => 0;
    public int Poll(TimeSpan timeout) => 0;

    public void InitTransactions(TimeSpan timeout) { }
    public void BeginTransaction() { }
    public void CommitTransaction() { }
    public void CommitTransaction(TimeSpan timeout) { }
    public void AbortTransaction() { }
    public void AbortTransaction(TimeSpan timeout) { }
    public void SendOffsetsToTransaction(IEnumerable<TopicPartitionOffset> offsets, IConsumerGroupMetadata groupMetadata, TimeSpan timeout) { }
    public void SetSaslCredentials(string username, string password) { }

    public Task<DeliveryResult<string, string>> ProduceAsync(string topic, Message<string, string> message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new DeliveryResult<string, string>());
    }

    public Task<DeliveryResult<string, string>> ProduceAsync(TopicPartition topicPartition, Message<string, string> message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new DeliveryResult<string, string>());
    }

    public void Produce(string topic, Message<string, string> message, Action<DeliveryReport<string, string>>? deliveryHandler = null)
    {
        deliveryHandler?.Invoke(new DeliveryReport<string, string>());
    }

    public void Produce(TopicPartition topicPartition, Message<string, string> message, Action<DeliveryReport<string, string>>? deliveryHandler = null)
    {
        deliveryHandler?.Invoke(new DeliveryReport<string, string>());
    }
}
