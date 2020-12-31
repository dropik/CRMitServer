using Microsoft.AspNetCore.Mvc;
using CRMitServer.Api;
using CRMitServer.Exceptions;

namespace CRMitServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IApplication application;
        
        public PurchaseController(IApplication application)
        {
            this.application = application;
        }

        public IActionResult Purchase(int clientId, int itemId)
        {
            try
            {
                application.HandlePurchaseRequestAsync(clientId, itemId);
                return Ok();
            }
            catch (RequestException)
            {
                return BadRequest();
            }
        }
    }
}
