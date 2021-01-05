using Microsoft.AspNetCore.Mvc;
using CRMitServer.Api;
using CRMitServer.Exceptions;
using System.Threading.Tasks;
using CRMitServer.Models;

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

        public async Task<IActionResult> Purchase(PurchaseRequest request)
        {
            try
            {
                await application.HandlePurchaseRequestAsync(request.ClientId, request.ItemId);
                return Ok();
            }
            catch (RequestException)
            {
                return BadRequest();
            }
        }
    }
}
