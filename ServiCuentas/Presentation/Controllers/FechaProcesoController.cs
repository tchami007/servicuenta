using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.Services;
using ServiCuentas.Model;

namespace ServiCuentas.Presentation.Controllers
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
        /// <summary>
        /// GetFechaProcesoById: Obtiene la fecha de proceso del id indicado por parametro
        /// </summary>
        /// <param name="id">integer que representa el id de fecha solicitado</param>
        /// <returns>201 - objeto fecha de proceso</returns>
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
        /// <summary>
        /// AddFechaProceso: Registra una nueva fecha de proceso
        /// </summary>
        /// <param name="fecha">Objeto de tipo fecha de proceso</param>
        /// <returns>201 - objeto de tipo fecha de proceso creado</returns>
        [HttpPost]
        public async Task<ActionResult> AddFechaProceso([FromBody] FechaProceso fecha)
        {
            var resultado = await _service.AddFechaProceso(fecha);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessages);
            }
            return CreatedAtAction("GetFechaProcesoById", new { fecha.IdFechaProceso }, resultado._value);
        }
        /// <summary>
        /// DeleteFechaProceso: Elimina una fecha de proceso del id indicado por parametro
        /// </summary>
        /// <param name="id">integer que representa el id de fecha solicitado</param>
        /// <returns>204 - No Content</returns>
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
        /// <summary>
        /// UpdateFechaProceso: Modifica una fecha de proceso indicada por un id de identificacion y un dto con los datos de la fecha de proceso
        /// </summary>
        /// <param name="id">integer que representa el id de fecha solicitado a modificar</param>
        /// <param name="fecha">objeto fecha de proceso con los datos a modificar</param>
        /// <returns>204 - no Content</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFechaProceso(int id, [FromBody] FechaProceso fecha)
        {
            var resultado = await _service.UpdateFechaProceso(id, fecha);
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
        /// GetAllFechasProceso: Recupera lista de fechas registradas
        /// </summary>
        /// <returns>Lista de fecha de proceso</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllFechasProceso()
        {
            var resultado = await _service.GetAll();
            return Ok(resultado);
        }

    }
}

