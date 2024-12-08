using FluentValidation;
using ServiCuentas.Application.DTOs;

namespace ServiCuentas.Application.Validators
{
    public class CuentaValidator : AbstractValidator<CuentaDTO>
    {
        public CuentaValidator() 
        {
            RuleFor(x=>x.FechaAlta).LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de alta debe ser menor o igual a la del dia");
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripcion no debe ser vacia");
            RuleFor(x => x.Capital).Equal(0).WithMessage("El capital inicialmente debe ser 0");
            RuleFor(x => x.Interes).Equal(0).WithMessage("El interes inicialmente debe ser 0");
            RuleFor(x => x.Estado).Equal(0).WithMessage("El estado inicialmente debe ser 0");
        }
    }
}
