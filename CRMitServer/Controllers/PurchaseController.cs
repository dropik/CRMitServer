using Microsoft.AspNetCore.Mvc;
using CRMitServer.Core;

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
                application.HandlePurchaseRequest(clientId, itemId);
                return Ok();
            }
            catch (RequestException)
            {
                return BadRequest();
            }
        }
    }
}
