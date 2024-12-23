using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services.FuncionServices;

namespace ServiCuentas.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionCreditoController : ControllerBase
    {
        private readonly IFuncionCredito _funcion;

        public FuncionCreditoController(IFuncionCredito funcion)
        {
            _funcion = funcion;
        }
        /// <summary>
        /// Credito Ejecutar: Incrementa el saldo (Capital) de la cuenta en el importe dado.
        /// </summary>
        /// <param name="funcion">El DTO que recibe como parametro incluye la cuenta, la fecha, el importe. Codigo de movimiento debe ser 0 para credito.</param>
        /// <returns></returns>
        [HttpPost("Ejecutar")]
        public async Task<ActionResult> Ejecutar(FuncionRequestAsientoDTO funcion)
        {
            var resultado = await _funcion.Ejecutar(funcion);
            if (!resultado._success)
            {
                //return BadRequest($"Error en la ejecucion CREDITO: {resultado._errorMessage}");
                return BadRequest(resultado);
            }
            return Ok(resultado._value);
        }
        /// <summary>
        /// Credito Contrasentar: Reversa el credito realizado con anterioridad (contrasiento) de forma
        /// que el saldo (Capital) disminuye por el importe de la funcion original
        /// </summary>
        /// <param name="funcion">El DTO que recibe como parametro incluye la cuenta, la fecha, el importe, el numero de comprobante original. Codigo de movimiento debe ser 0 para credito.</param>
        /// <returns></returns>
        [HttpPost("Contrasentar")]
        public async Task<ActionResult> Contrasiento(FuncionRequestDTO funcion)
        {
            var resultado = await _funcion.Contrasentar(funcion);
            if (!resultado._success)
            {
                //return BadRequest($"Error en la ejecucion CONTRASIENTO CREDITO: {resultado._errorMessage}");
                return BadRequest(resultado);

            }
            return Ok(resultado._value);
        }
    }
}
