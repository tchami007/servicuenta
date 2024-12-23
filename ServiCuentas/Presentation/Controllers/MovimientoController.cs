using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services;

namespace ServiCuentas.Presentation.Controllers
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
        /// <summary>
        /// GetMovimientoById: Recupera un movimiento indicado por su identificador
        /// </summary>
        /// <param name="id">integer que representa el id del movimiento solicitado</param>
        /// <returns>200 - objeto de tipo movimiento</returns>
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
        /// <summary>
        /// AddMovimiento: Registra un movimiento a partir de un objeto con los datos a registrar
        /// </summary>
        /// <param name="movimiento">objeto de tipo movimiento con los datos a registrar</param>
        /// <returns>200 - objeto movimiento con los datos del nuevo registro</returns>
        [HttpPost]
        public async Task<ActionResult> AddMovimiento([FromBody] MovimientoDTO movimiento)
        {
            var resultado = await _service.AddMovimiento(movimiento);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            return CreatedAtAction("GetMovimientoById", new { movimiento.IdMovimiento }, resultado._value);
        }
        /// <summary>
        /// DeleteMovimiento: Elimina el registro de un movimiento a partir del identificador solicitado
        /// </summary>
        /// <param name="id">integer que representa el identificador del movimiento a eliminar</param>
        /// <returns>204 - no content</returns>
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
        /// <summary>
        /// UpdateMovimiento: Modifica el movimiento a partir de un identificador y un objeto con los nuevos datos del movimiento 
        /// </summary>
        /// <param name="id">integer que representa el identificador del movimiento a modificar</param>
        /// <param name="movimiento">objeto movimiento con los nuevos datos a asignar</param>
        /// <returns>204 - no content</returns>
        [HttpPut("id")]
        public async Task<ActionResult> UpdateMovimiento(int id, [FromBody] MovimientoDTO movimiento)
        {
            var resultado = await _service.UpdateMovimiento(id, movimiento);
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
        /// GetAllMovimiento: Recupera la lista de movientos a partir de una serie de filtros. Aplica ordenamiento y paginacion.
        /// </summary>
        /// <param name="filter">objeto filtro que permite definir datos a segregar de la consulta</param>
        /// <param name="order">string que representa el ordenamiento.</param>
        /// <param name="page">integer que representa el numero de pagina a solicitar</param>
        /// <param name="pageSize">integer que representa la cantidad de registros por pagina a solicitar</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAllMovimiento(
            [FromQuery] MovimientoFilterDTO filter,
            [FromQuery] string order = "numerocuenta",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var resultado = await _service.GetAllMovimiento(filter, order, page, pageSize);
            return Ok(resultado);
        }
    }
}
