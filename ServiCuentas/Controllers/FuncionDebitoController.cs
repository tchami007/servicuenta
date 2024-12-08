using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services.FuncionServices;

namespace ServiCuentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionDebitoController : ControllerBase
    {
        private readonly IFuncionDebito _funcion;

        public FuncionDebitoController(IFuncionDebito funcion)
        {
            _funcion = funcion;
        }
        /// <summary>
        /// Debito Ejecutar: Decrementa el saldo (capital) de la cuenta por importe recibido.
        /// </summary>
        /// <param name="funcion">El DTO que recibe como parametro incluye la cuenta, la fecha el importe. El codigo de movimiento debe ser 1 para debito.</param>
        /// <returns></returns>
        [HttpPost("Ejecutar")]
        public async Task<ActionResult> Ejecutar(FuncionRequestAsientoDTO funcion)
        {
            var resultado = await _funcion.Ejecutar(funcion);
            if (!resultado._success)
            {
                return BadRequest($"Error en la ejecucion DEBITO: {resultado._errorMessage}");
            }
            return Ok(resultado._value);
        }
        /// <summary>
        /// Debito Contrasientar: Reversa el debito restaurando el saldo (capital) de la cuenta por el importe del movimiento original.
        /// </summary>
        /// <param name="funcion">El DTO que recibe como parametro incluye la cuenta, la fecha, el importe, el numero de comprobante original. Codigo de movimiento debe ser 1 para debito.</param>
        /// <returns></returns>
        [HttpPost("Contrasentar")]
        public async Task<ActionResult> Contrasiento(FuncionRequestDTO funcion)
        {
            var resultado = await _funcion.Contrasentar(funcion);
            if (!resultado._success)
            {
                return BadRequest($"Error en la ejecucion CONTRASIENTO DEBITO: {resultado._errorMessage}");

            }
            return Ok(resultado._value);
        }

    }
}
