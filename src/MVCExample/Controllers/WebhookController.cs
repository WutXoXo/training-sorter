using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;

namespace MVCExample.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("collection/mail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MailCollection(object data)
        {
            int channelId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _logger.LogInformation($"Channel Id:{channelId}");
            _logger.LogInformation(data.ToString());

            return Ok();
        }

        [HttpPost]
        [Route("delivery/proof")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ProofOfDelivery(object data)
        {
            int channelId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _logger.LogInformation($"Channel Id:{channelId}");
            _logger.LogInformation(data.ToString());

            return Ok();
        }
    }
}
