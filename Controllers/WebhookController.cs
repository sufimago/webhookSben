using Microsoft.AspNetCore.Mvc;
using webhookSben.DTO;
using webhookSben.KafkaProducer;

namespace webhookSben.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly KafkaProducerService _kafkaProducer;

        private readonly ILogger<WebhookController> _logger;

        public WebhookController(KafkaProducerService kafkaProducer, ILogger<WebhookController> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CancelarReserva([FromBody] ProviderData request)
        {
            if (request == null)
                return BadRequest("El cuerpo de la solicitud no puede ser nulo.");

            if (string.IsNullOrEmpty(request.EventType) || request.ListingId == 0)
                return BadRequest("Datos incompletos en la solicitud.");

            try
            {
                // Enviar el evento a Kafka
                await _kafkaProducer.ProduceAsync(request);

                return Ok(new { mensaje = "Evento recibido y enviado a Kafka correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al enviar a Kafka.", error = ex.Message });
            }
        }
    }
}
