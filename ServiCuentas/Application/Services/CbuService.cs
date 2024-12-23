using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Shared;
using ServiCuentas.Shared.Shared;

namespace ServiCuentas.Application.Services
{
    public interface ICbuService
    {
        Task<Result<string>> CalcularCBU(decimal numeroCuenta);
    }
    public class CbuService : ICbuService
    {
        private readonly ICbuCalculator _calculator;
        private readonly ICuentaRepository  _cuentaRepository;

        public CbuService (ICbuCalculator calculator, ICuentaRepository cuenta)
        {
            _calculator = calculator;
            _cuentaRepository = cuenta;
        }

        public async Task<Result<string>> CalcularCBU(decimal numeroCuenta)
        {
            string numeroCuentaString = numeroCuenta.ToString();

            var retorno = _calculator.GenerarCbu(numeroCuentaString);

            // Busqueda de la cuenta

            var cuenta = await _cuentaRepository.GetCuentaByNumeroCuenta(numeroCuenta);

            if (!cuenta._success) 
            {
                return Result<string>.Failure($"Error: La cuenta no existe - {cuenta._errorMessage}");
            }

            // Actualizar CBU en la cuenta

            cuenta._value.Cvu = retorno;

            var resultado = await _cuentaRepository.UpdateItem(cuenta._value);
            if (!resultado._success)
            {
                return Result<string>.Failure($"Error: No se pudo realizar la actualizacion - {cuenta._errorMessage}");
            }

            return Result<string>.Success(retorno);
        }
    }
}
