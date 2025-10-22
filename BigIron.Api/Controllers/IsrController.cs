using BigIron.Api.DTOs;
using Core.Adapter;
using Core.DTOs;
using Core.Models;
using Core.Services;
using Core.ValueObjects;
using Core.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BigIronApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsrController(IISRCsvReader reader, IISRService service) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<Response<List<ISR>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ProcessData([FromForm] ISRProcessDataRequest request)
        {
            try
            {
                var processedData = service.GetVisitationRoute(request);

                return Ok(processedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
