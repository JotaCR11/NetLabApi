using Netlab.Business.Services;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Netlab.WebApp.Controllers
{
    [Route("api/notiweb")]
    [ApiController]
    [Authorize]
    public class RegistrarNotiWebController : ControllerBase
    {
        private readonly IRegistrarNotiWebService _registrarService;
        public RegistrarNotiWebController(IRegistrarNotiWebService registrarService)
        {
            _registrarService = registrarService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] DatoNotiwebNetlab request)
        {
            if (!String.IsNullOrWhiteSpace(request.cod_etnia_pac) && request.cod_etnia_pac.ToLower() == "otro" && String.IsNullOrWhiteSpace(request.otroetniaproc_pac))
            {
                throw new ArgumentNullException("otroetniaproc_pac", $"este campo es obligatorio porque enviaron cod_etnia_pac = {request.cod_etnia_pac}");
            }

            //validar valores aceptados


            DatoNotiwebNetlabResponse registrarResponse = await _registrarService.RegistrarAsync(request);
            //validar si el response del servicio es exitoso o no
            //si es exitoso retornar Ok, sino un BadRequest
            Boolean responseSuccess = registrarResponse.responseId != 0;
            if (responseSuccess)
            {
                return Ok(new
                {
                    codigo_cdc = registrarResponse.codigo_cdc,
                    codigo_INS = registrarResponse.codigo_INS
                });
            }
            else
            {
                return BadRequest(new { errors = new String[] { "Error en registro de información."} });
            }
        }

        [HttpPost("testRecibeCdc")]
        public async Task<IActionResult> testRecibeCdc([FromBody] TestRequest request)
        {
            LogTestCDC resultado = await _registrarService.TestRegistroCDCAsync(request);
            return Ok(resultado);
        }
    }
}
