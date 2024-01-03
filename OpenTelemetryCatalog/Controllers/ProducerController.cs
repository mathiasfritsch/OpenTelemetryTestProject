using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Diagnostics;

namespace OpenTelemetryCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;
    private readonly CatalogContext _catalogContext;
    private readonly IBus _bus;

    private static readonly ActivitySource _activitySource = new("producer-service");

    public ProducerController(IBus bus, ILogger<ProducerController> logger, CatalogContext catalogContext)
    {

        _logger = logger;
        _catalogContext = catalogContext;
        _bus = bus;
    }


    [HttpGet("testcontext")]
    public async Task<string> TestContext()
    {

        var product1 = await _catalogContext.Products.FirstAsync();

        return product1.Name;

    }


    [HttpPost("send")]
    public async Task Send()
        => await _bus.Publish(new ContractDto { Message = $"The time is {DateTimeOffset.Now}" });



    [HttpPost("manualactivity")]
    public void Manual()
    {
        var activity = Activity.Current;



    }

    private void ChildMethod()
    {
        // using var activity = _activitySource.StartActivity("Child method");

    }

}