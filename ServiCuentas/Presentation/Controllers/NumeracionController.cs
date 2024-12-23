using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.Services;
using ServiCuentas.Model;

namespace ServiCuentas.Presentation.Controllers
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
        /// <summary>
        /// GetNumeracionById: Recupera el registro de una numeracion por su identificador
        /// </summary>
        /// <param name="id">integer que representa el identificador a recuperar</param>
        /// <returns>Registro con los datos de la numeracion solicitada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetNumeracionById(int id)
        {
            var resultado = await _service.GetNumeracionById(id);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if (resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }
        /// <summary>
        /// AddMovimiento: Agrega un nuevo registro de numeración
        /// </summary>
        /// <param name="numeracion">Objeto del modelo para registrar nueva numeracion</param>
        /// <returns>Nuevo registro de numeración generado</returns>
        [HttpPost]
        public async Task<ActionResult> AddNumeracion([FromBody] Numeracion numeracion)
        {
            var resultado = await _service.AddNumeracion(numeracion);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            return CreatedAtAction("GetNumeracionById", new { id = numeracion.IdNumeracion }, resultado._value);
        }
        /// <summary>
        /// DeleteNumeracion: Elimina un registro de numeracion por su identificador
        /// </summary>
        /// <param name="id">integer que representa el identificador eliminado</param>
        /// <returns>Status 204</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNumeracion(int id)
        {
            var resultado = await _service.DeleteNumeracion(id);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            if (resultado == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        /// <summary>
        /// UpdateNumeracion: Modifica el registro de numeracion identificado con los datos del objeto del modelo dado
        /// </summary>
        /// <param name="id">integer que representa el identificador a modificar</param>
        /// <param name="numeracion">objeto que contiene los nuevos valores a aplicar</param>
        /// <returns>Status 204</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNumeracion(int id, [FromBody] Numeracion numeracion)
        {
            var resultado = await _service.UpdateNumeracion(id, numeracion);
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
        /// <summary>
        /// GetAllNumeracion: Recupera la lista completa de registros de numeracion
        /// </summary>
        /// <returns>Lista de numeraciones</returns>
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
            if (resultado._value == null)
            {
                return NotFound();
            }
            return Ok(resultado._value);
        }
    }
}
