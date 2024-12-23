using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services;

namespace ServiCuentas.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _service;
        private readonly ICbuService _cbuService;

        public CuentaController(ICuentaService service, ICbuService cbuService)
        {
            _service = service;
            _cbuService = cbuService;
        }

        /// <summary>
        /// GetCuentasById: Obtiene los datos de una cuenta identificada por un id (identificador)
        /// </summary>
        /// <param name="id">Identificador interno de la cuenta</param>
        /// <returns>Datos de una cuenta.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDTO>> GetCuentaById(int id)
        {
            var resultado = await _service.GetCuentaById(id);
            // Devuelve 404 si no encuentra
            if (!resultado._success || resultado._value == null)
            {
                return NotFound();
            }
            // Devuelve 200 si funciona ok
            return Ok(resultado._value);
        }
        /// <summary>
        /// GetAllCuentas: Obtiene el listado de cuentas con filtros, ordenamiento y paginación
        /// </summary>
        /// <param name="Filter">Objeto DTO con las opciones de filtrado</param>
        /// <param name="order">String que permite definir las alternativas de ordenamiento. Valores funcionales: "numerocuenta", "descripcion", "fechaalta"</param>
        /// <param name="page">int que indica el numero de pagina a solicitar</param>
        /// <param name="pageSize">int que indica el numero de registros por pagina a incluir en cada pagina</param>
        /// <returns>Lista de objetos de tipo CuentaDTO con los datos de las cuentas</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCuentas(
        [FromQuery] CuentaFilterDTO Filter,
        [FromQuery] string order = "numerocuenta",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            var resultado = await _service.GetAllCuentas(Filter, order, page, pageSize);
            return Ok(resultado);
        }
        /// <summary>
        /// AddCuenta: Crea una nueva cuenta
        /// </summary>
        /// <param name="descripcion">Dato de tipo string que sirve para describir la cuenta</param>
        /// <returns></returns>
        [HttpPost("{descripcion}")]
        public async Task<ActionResult<CuentaDTO>> AddCuenta(string descripcion)
        {
            var resultado = await _service.AddCuenta(descripcion);
            // Devuelve error
            if (!resultado._success || resultado._value == null)
            {
                return BadRequest(resultado._errorMessages);
            }
            // Devuelve nuevo recurso si funciona
            return CreatedAtAction("GetCuentaById", new { id = resultado._value.IdCuenta }, resultado._value);
        }
        /// <summary>
        /// AddCuenta: Crea una nueva cuenta dado un dto
        /// </summary>
        /// <param name="cuenta">DTO que contiene los datos necesarios para la creacion de una cuenta</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CuentaDTO>> AddCuenta([FromBody] CuentaDTO cuenta)
        {
            var resultado = await _service.AddCuenta(cuenta);
            // Devuelve error
            if (!resultado._success || resultado._value == null)
            {
                return BadRequest(resultado._errorMessages);
            }
            // Devuelve nuevo recurso si funciona
            return CreatedAtAction("GetCuentaById", new { id = cuenta.IdCuenta }, resultado._value);
        }
        /// <summary>
        /// UpdateCuenta: Modifica los datos de una cuenta dado el id de cuenta y un DTO con los nuevos datos a asignar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCuenta(int id, [FromBody] CuentaDTO cuenta)
        {
            var resultado = await _service.UpdateCuenta(id, cuenta);
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessage);
            }
            // Devuelve 404 si no encuentra
            if (resultado._value == null)
            {
                return NotFound();
            }
            // Devuelve 204 si funciono ok
            return NoContent();
        }
        /// <summary>
        /// DeleteCuenta: Elimina una cuenta creada, mediante su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCuenta(int id)
        {
            var resultado = await _service.DeleteCuenta(id);
            // devuelve error si no funciona
            if (!resultado._success)
            {
                return BadRequest(resultado._errorMessages);
            }
            // devuelve 404 si no encuentra
            if (resultado._value == null)
            {
                return NotFound();
            }
            // devuelve 204 si funciona ok
            return NoContent();
        }
        /// <summary>
        /// GetCuentaByNumeroCuenta: Recupera la cuenta con el numero de cuenta dado.
        /// </summary>
        /// <param name="numeroCuenta">decimal: Numero de la cuenta a buscar</param>
        /// <returns>cuentaDTO: objeto con los datos de la cuenta buscada</returns>
        [HttpGet("por_numerocuenta")]
        public async Task<ActionResult<CuentaDTO>> GetCuentaByNumeroCuenta([FromQuery] decimal numeroCuenta)
        {
            var resultado = await _service.GetCuentaByNumeroCuenta(numeroCuenta);
            if (resultado._success)
            {
                return Ok(resultado._value);
            }
            return NotFound(resultado);
        }
        /// <summary>
        /// GetCuentasByCelular: Recupera la lista de cuentas que responden al celular de registro dado.
        /// </summary>
        /// <param name="celular">string: indica el numero de celcular de busqueda</param>
        /// <returns>Lista de Cuenta DTO que responde al email dado</returns>
        [HttpGet("por_celular")]
        public async Task<ActionResult> GetCuentasByCelular([FromQuery] string celular)
        {
            var resultado = await _service.GetCuentasByCelular(celular);
            return Ok(resultado);
        }
        /// <summary>
        /// GentCuentasByEmail: Reculera la lista de cuentas que responden al email de registro dado
        /// </summary>
        /// <param name="email">string: indica la direccion de correo de busqueda</param>
        /// <returns>Lista de CuentaDTO que responde al celular dado</returns>
        [HttpGet("por_email")]
        public async Task<ActionResult> GetCuentasByEmail([FromQuery] string email)
        {
            var resultado = await _service.GetCuentasByEmail(email);
            return Ok(resultado);
        }
        /// <summary>
        /// GetCuentaByCvu: Recupera la cuenta que responde al cvu de registro dado
        /// </summary>
        /// <param name="cvu">string: Indica el valor del cvu de busqueda</param>
        /// <returns>CuentaDTO: objeto con los datos de la cuenta buscada</returns>
        [HttpGet("por_cvu")]
        public async Task<ActionResult<CuentaResponseDTO>> GetCuentaByCvu([FromQuery] string cvu)
        {
            var resultado = await _service.GetCuentaByCvu(cvu);
            if (resultado._success)
            {
                return Ok(resultado._value);
            }
            return NotFound(resultado);
        }
        /// <summary>
        /// GetCuentaByAlias: Recupera la cuenta que responde al alias de registro dado
        /// </summary>
        /// <param name="alias">string: Indica el valor del alias de busqueda</param>
        /// <returns>CuentaResponseDTO: objeto con los datos de la cuenta buscada</returns>
        [HttpGet("por_alias")]
        public async Task<ActionResult<CuentaResponseDTO>> GetCuentaByAlias([FromQuery] string alias)
        {
            var resultado = await _service.GetCuentaByAlias(alias);
            if (resultado._success)
            {
                return Ok(resultado._value);
            }
            return NotFound(resultado);
        }

        /// <summary>
        /// CalcularCvu: Realiza el calculo y actualizacion del cvu de la cuenta
        /// </summary>
        /// <param name="numeroCuenta">decimal que indica el numero de la cuenta</param>
        /// <returns>objeto con el numero de cvu generado</returns>
        [HttpPost("CalcularCvu")]
        public async Task<ActionResult> CalcularCvu(decimal numeroCuenta)
        {
            var resultado = await _cbuService.CalcularCBU(numeroCuenta);
            if (!resultado._success)
            {
                return NotFound(resultado._errorMessages);
            }
            return Ok(resultado);
        }
    }
}
