using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreMastersTodoList.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}