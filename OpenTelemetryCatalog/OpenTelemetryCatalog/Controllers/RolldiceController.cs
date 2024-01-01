using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace OpenTelemetryCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolldiceController : ControllerBase
    {
        private readonly ILogger<RolldiceController> _logger;
        private readonly IBus _bus;

        public RolldiceController(IBus bus, ILogger<RolldiceController> logger)
        {
            _logger = logger;
            _bus = bus;
        }

        private int RollDice() => Random.Shared.Next(1, 7);


        [HttpPost("send")]
        public async Task Send()
            => await _bus.Publish(new ContractDto { Message = $"The time is {DateTimeOffset.Now}" });

        [HttpGet("{player?}")]
        public string Get(string? player)
        {
            var result = RollDice();

            if (string.IsNullOrEmpty(player))
            {
                _logger.LogInformation("Anonymous player is rolling the dice: {result}", result);
            }
            else
            {
                _logger.LogInformation("{player} is rolling the dice: {result}", player, result);
            }

            return $"ok {player}";
        }
    }
}


