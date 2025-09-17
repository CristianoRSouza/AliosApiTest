using Ailos.Shared.Common.Interfaces;

namespace Ailos.ContaCorrente.API.Services;

public class NoOpEventPublisher : IEventPublisher
{
    public Task PublishAsync<T>(string topic, T eventData) where T : class
    {
        // Não faz nada - implementação vazia para desenvolvimento
        return Task.CompletedTask;
    }
}
