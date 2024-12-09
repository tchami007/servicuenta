using FluentValidation;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;

namespace ServiCuentas.Application.Validators
{
    public class FuncionRequestAsientoValidator : AbstractValidator<FuncionRequestAsientoDTO>
    {
        private readonly ICuentaRepository _cuenta;
        private readonly IFechaProcesoRepository _fecha;
        public FuncionRequestAsientoValidator(ICuentaRepository cuenta, IFechaProcesoRepository fecha)
        {
            var validContrasiento = new List<string> { "", "C" };
            var validCodigoMovimiento = new List<int> { 0, 1 };
            _cuenta = cuenta;
            _fecha = fecha;

            //Validacion de NumeroCuenta
            RuleFor(x => x.NumeroCuenta)
                .NotEmpty()
                .WithErrorCode("1000")
                .WithMessage("El numero de cuenta no de debe ser vacio")
                .MustAsync(async (numeroCuenta, cancelation) => await _cuenta.ExisteNumeroCuenta(numeroCuenta))
                .WithErrorCode("1001")
                .WithMessage("El numero de cuenta no existe");
            // Validacion de FechaMovimiento
            RuleFor(x => x.FechaMovimiento)
                .MustAsync(async (fec, cancelation) => await _fecha.EsFechaProceso(fec,"CUENTA"))
                .WithErrorCode("1002")
                .WithMessage($"La fecha de movimiento debe ser la fecha de proceso");
            // Validacion para CodigoMovimiento
            RuleFor(x => x.CodigoMovimiento)
                .Must(cod => validCodigoMovimiento.Contains(cod))
                .WithErrorCode("1003")
                .WithMessage("El codigo de movimiento debe ser 0 (debito) o 1(credito)");
            // Validacion de Importe
            RuleFor(x => x.Importe)
                .NotEmpty()
                .WithErrorCode("1004")
                .WithMessage("El importe debe ser ingresado")
                .GreaterThan(0)
                .WithErrorCode("1005")
                .WithMessage("El importe debe ser positivo");
}
    }
}
