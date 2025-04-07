using Messaging.RabitMQ.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        private readonly IMessagePublisher _publisher;

        public RabbitController(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            _publisher.PublishMessage(message, "emri-i-queues");
            return Ok("Mesazhi u dërgua");
        }
    }
}
