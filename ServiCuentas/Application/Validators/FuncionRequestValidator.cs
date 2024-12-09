using FluentValidation;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Infraestructure.Repository;

namespace ServiCuentas.Application.Validators
{
    public class FuncionRequestValidator : AbstractValidator<FuncionRequestDTO>
    {
        private readonly ICuentaRepository _cuenta;
        private readonly IFechaProcesoRepository _fecha;
        public FuncionRequestValidator(ICuentaRepository cuenta, IFechaProcesoRepository fecha) 
        {
            var validContrasiento = new List<string> { "", "C" };
            var validCodigoMovimiento = new List<int> { 0, 1 };
            _cuenta = cuenta;
            _fecha = fecha;

            //Validacion de NumeroCuenta
            RuleFor(x => x.NumeroCuenta)
                .NotEmpty()
                .WithErrorCode("1000")
                .WithMessage("El numero de cuenta no de debe ser vacion")
                .MustAsync(async (numeroCuenta,cancelation) => await _cuenta.ExisteNumeroCuenta(numeroCuenta))
                .WithErrorCode("1001")
                .WithMessage("El numero de cuenta no existe");
            // Validacion de FechaMovimiento
            RuleFor(x=>x.FechaMovimiento)
                .MustAsync(async (fec, cancelation) => await _fecha.EsFechaProceso(fec, "CUENTA"))
                .WithErrorCode("1002")
                .WithMessage("La fecha de movimiento debe ser la fecha de proceso");
            // Validacion de contrasiento
            RuleFor(x => x.Contrasiento.ToString())
                .Must(cont => validContrasiento.Contains(cont))
                .WithErrorCode("1003")
                .WithMessage("Contrasiento debe ser cadeana vacia o 'C'");
            // Validación para NumeroComprobante basado en Contrasiento
            RuleFor(x => x.NumeroComprobante)
                .NotEmpty()
                .WithErrorCode("1004")
                .WithMessage("El número de comprobante es obligatorio cuando Contrasiento es 'C'.")
                .When(x => x.Contrasiento == "C");
            // Validacion para CodigoMovimiento
            RuleFor(x => x.CodigoMovimiento)
                .Must(cod => validCodigoMovimiento.Contains(cod))
                .WithErrorCode("1005")
                .WithMessage("El codigo de movimiento debe ser 0 (debito) o 1(credito)");
            // Validacion de Importe
            RuleFor(x => x.Importe)
                .NotEmpty()
                .WithErrorCode("1006")
                .WithMessage("El importe debe ser ingresado")
                .GreaterThan(0)
                .WithErrorCode("1007")
                .WithMessage("El importe debe ser positivo");

        }
    }
}
