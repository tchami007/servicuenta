using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.Services.BatchServices;
using ServiCuentas.Shared;

namespace ServiCuentas.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchProcessController : ControllerBase
    {
        private readonly IBatchProcess<BatchProcessConfig> _process;
        private readonly BatchProcessConfig _config;

        public BatchProcessController(IBatchProcess<BatchProcessConfig> process, BatchProcessConfig config)
        {
            _process = process;
            _config = config ?? throw new ArgumentNullException(nameof(config)); 
        }

        [HttpPost("CalculoProcess")]
        public async Task<ActionResult> CalculoProcess()
        {
            var resultado = await _process.CalculoProcess(_config);

            if (!resultado._success)
            {
                return NotFound(resultado._errorMessage);
            }

            return Ok(resultado);
        }

        [HttpPost("DevengamientoInteresProcess")]
        public async Task<ActionResult> DevengamientoInteresProcess()
        {
            var resultado = await _process.DevengamientoInteresProcess(_config);

            if (!resultado._success)
            {
                return NotFound(resultado._errorMessage);
            }

            return Ok(resultado);
        }

        [HttpPost("ComisionesProcess")]
        public async Task<ActionResult> ComisionesProcess()
        {
            var resultado = await _process.ComisionesProcess(_config);

            if (!resultado._success)
            {
                return NotFound(resultado._errorMessage);
            }

            return Ok(resultado);
        }

        [HttpPost("CierreFechaProcess")]
        public async Task<ActionResult> CierreFechaProcess()
        { 
            var resultado = await _process.CierreFechaProcess(_config);

            if (!resultado._success)
            {
                return NotFound(resultado._errorMessage);
            }

            return Ok(resultado);
        }

        [HttpPost("AperturaFechaProcess")]
        public async Task<ActionResult> AperturaFechaProcess()
        {
            var resultado = await _process.AperturaFechaProcess(_config);

            if (!resultado._success)
            {
                return NotFound(resultado._errorMessage);
            }

            return Ok(resultado);
        }
    }
}
