using MassTransit;
using Shared;

namespace ConsumerApi;

public class MessageConsumer : IConsumer<ContractDto>

{
    readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(
        ILogger<MessageConsumer> logger
    )
    {
        _logger = logger;
    }


    public Task Consume(ConsumeContext<ContractDto> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message.Message);
        return Task.CompletedTask;
    }
}

