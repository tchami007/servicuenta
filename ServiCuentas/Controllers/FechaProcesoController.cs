using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.Services;
using ServiCuentas.Model;

namespace ServiCuentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FechaProcesoController : ControllerBase
    {
        private readonly IFechaProcesoService _service;

        public FechaProcesoController(IFechaProcesoService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFechaProcesoById(int id)
        { 
            var resultado = await _service.GetFechaProcesoById(id);
            if (!resultado._success)
            { 
                return BadRequest(resultado._errorMessages);
            }
            if (resultado._value == null)
            { 
                return NotFound();
            }
            return Ok(resultado);
        }
        [HttpPost]
        public async Task<ActionResult> AddFechaProceso([FromBody]FechaProceso fecha)
        {
            var resultado = await _service.AddFechaProceso(fecha);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessages);
            }
            return CreatedAtAction("GetFechaProcesoById", new { IdFechaProceso = fecha.IdFechaProceso }, resultado._value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFechaProceso(int id)
        {
            var resultado = await _service.DeleteFechaProceso(id);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if (resultado._value == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFechaProceso(int id, [FromBody] FechaProceso fecha)
        {
            var resultado = await _service.UpdateFechaProceso(id, fecha);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if (resultado._value==null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult> GetAllFechasProceso()
        {
            var resultado = await _service.GetAll();
            return Ok(resultado);
        }

    }
}

