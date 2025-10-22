using Core.Adapter;
using Core.DTOs;
using Core.Models;
using Core.Services;
using Core.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BigIronApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsrController(ISRAdapter adapter, ISRService service) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<Response<List<ISR>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ProcessData([FromForm] IFormFile formFile)
        {

            if (formFile is null || formFile.Length == 0)
            {
                return BadRequest("File is empty or missing.");
            }

            if (formFile.ContentType != "text/csv")
            {
                return BadRequest("Invalid type");
            }
            try
            {
                using var stream = formFile.OpenReadStream();
                
                List<ISRDTO> data = adapter.ReadFile(stream);

                var processedData = service.ProcessData(data);

                return Ok(processedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
