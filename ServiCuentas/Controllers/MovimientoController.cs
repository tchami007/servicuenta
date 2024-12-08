using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services;

namespace ServiCuentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoService _service;
        public MovimientoController(IMovimientoService service)
        {
            _service = service;
        }   
        [HttpGet("id")]
        public async Task<ActionResult> GetMovimientoById(int id)
        { 
            var resultado = await _service.GetMovimientoById(id);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if (resultado._value == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }
        [HttpPost]
        public async Task<ActionResult> AddMovimiento([FromBody] MovimientoDTO movimiento)
        {
            var resultado = await _service.AddMovimiento(movimiento);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            return CreatedAtAction("GetMovimientoById", new { IdMovimiento = movimiento.IdMovimiento }, resultado._value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovimiento(int id)
        {
            var resultado = await _service.DeleteMovimiento(id);
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
        [HttpPut("id")]
        public async Task<ActionResult> UpdateMovimiento(int id, [FromBody] MovimientoDTO movimiento)
        {
            var resultado = await _service.UpdateMovimiento(id, movimiento);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if(resultado._value == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult> GetAllMovimiento(
            [FromQuery] MovimientoFilterDTO filter,
            [FromQuery] string order = "numerocuenta",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize=10)
        {
            var resultado = await _service.GetAllMovimiento(filter, order, page, pageSize);
            return Ok(resultado);
        }
    }
}
