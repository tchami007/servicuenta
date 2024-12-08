using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.Services;
using ServiCuentas.Model;

namespace ServiCuentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeracionController : ControllerBase
    {
        private readonly INumeracionService _service;
        public NumeracionController(INumeracionService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetNumeracionById(int id)
        { 
            var resultado = await _service.GetNumeracionById(id);
            if (!resultado._success)
            { 
                return BadRequest(resultado._errorMessage);
            }
            if(resultado == null) 
            { 
                return NotFound(); 
            }
            return Ok(resultado);
        }
        [HttpPost]
        public async Task<ActionResult> AddMovimiento([FromBody] Numeracion numeracion)
        {
            var resultado = await _service.AddNumeracion(numeracion);
            if (!resultado._success)
            { 
                return BadRequest(resultado._errorMessage);
            }
            return CreatedAtAction("GetNumeracionById", new { id = numeracion.IdNumeracion }, resultado._value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovimiento(int id)
        {
            var resultado = await _service.DeleteNumeracion(id);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if(resultado == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovimiento(int id, [FromBody] Numeracion numeracion)
        {
            var resultado = await _service.UpdateNumeracion(id, numeracion);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if(resultado._value== null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult> GetAllNumeracion()
        {
            var resultado = await _service.GetAllNumeracion();
            return Ok(resultado);
        }
        [HttpGet("descripcion")]
        public async Task<ActionResult> GetNumeracionByDescripcion(string descripcion)
        { 
            var resultado = await _service.GetNumeracionByDescripcion(descripcion);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if(resultado._value== null)
            {
                return NotFound();
            }
            return Ok(resultado._value);
        }
    }
}
