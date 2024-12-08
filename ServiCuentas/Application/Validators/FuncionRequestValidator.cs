using FluentValidation;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Infraestructure.Repository;

namespace ServiCuentas.Application.Validators
{
    public class FuncionRequestValidator : AbstractValidator<FuncionRequestDTO>
    {
        private readonly ICuentaRepository _cuentaRepository;
        public FuncionRequestValidator(ICuentaRepository cuenta) 
        {
            var validContrasiento = new List<string> { "", "C" };
            var validCodigoMovimiento = new List<int> { 0, 1 };
            _cuentaRepository = cuenta;

            //Validacion de NumeroCuenta
            RuleFor(x => x.NumeroCuenta)
                .NotEmpty()
                .WithMessage("El numero de cuenta no de debe ser vacion")
                .MustAsync(async (numeroCuenta,cancelation) => await cuenta.ExisteNumeroCuenta(numeroCuenta))
                .WithMessage("El numero de cuenta no existe");
            // Validacion de FechaMovimiento
            RuleFor(x=>x.FechaMovimiento)
                .Equal(DateTime.Today).WithMessage("La fecha de proceso debe ser la del dia");
            // Validacion de contrasiento
            RuleFor(x => x.Contrasiento.ToString())
                .Must(cont => validContrasiento.Contains(cont))
                .WithMessage("Contrasiento debe ser cadeana vacia o 'C'");
            // Validación para NumeroComprobante basado en Contrasiento
            RuleFor(x => x.NumeroComprobante)
                .NotEmpty().WithMessage("El número de comprobante es obligatorio cuando Contrasiento es 'C'.")
                .When(x => x.Contrasiento == "C");
            // Validacion para CodigoMovimiento
            RuleFor(x => x.CodigoMovimiento)
                .Must(cod => validCodigoMovimiento.Contains(cod))
                .WithMessage("El codigo de movimiento debe ser 0 (debito) o 1(credito)");
            // Validacion de Importe
            RuleFor(x => x.Importe)
                .NotEmpty()
                .WithMessage("El importe debe ser ingresado")
                .GreaterThan(0)
                .WithMessage("El importe debe ser positivo");

        }
    }
}
