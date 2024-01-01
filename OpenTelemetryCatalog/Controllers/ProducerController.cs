using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace OpenTelemetryCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;
    private readonly IBus _bus;

    public ProducerController(IBus bus, ILogger<ProducerController> logger)
    {
        _logger = logger;
        _bus = bus;
    }


    [HttpPost("send")]
    public async Task Send()
        => await _bus.Publish(new ContractDto { Message = $"The time is {DateTimeOffset.Now}" });


}