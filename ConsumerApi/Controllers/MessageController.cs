using Microsoft.AspNetCore.Mvc;

namespace ConsumerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{

    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetMessage")]
    public string Get()
    {
        return "ok";
    }
}
