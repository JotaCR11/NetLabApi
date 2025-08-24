using Netlab.Business.Services;
using Netlab.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Netlab.WebApp.Controllers
{
    [Route("api/resultados/arbovirosis")]
    [ApiController]
    public class ConsultaResultadosArbovirosisController : ControllerBase
    {
        private readonly IConsultaResultadosArbovirosisService _service;
        public ConsultaResultadosArbovirosisController(IConsultaResultadosArbovirosisService service)
        {
            _service = service;
        }

        [HttpGet("obtenerresultadosPorDNI")]
        public async Task<IActionResult> ObtenerResultadosPorDNI([FromBody] ResultadosArbovirosisRequest request)
        {
            var response = await _service.ObtenerResultadosArbovirosisPorDni(request.dni);
            return Ok(new { response });
        }
    }
}
