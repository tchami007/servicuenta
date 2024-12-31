using AutoMapper;
using FluentValidation;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Validators;
using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services.FuncionServices
{
    public class FuncionDebitoService : IFuncionDebito
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IValidator<FuncionRequestDTO> _validator;
        private readonly IValidator<FuncionRequestAsientoDTO> _validatorAsiento;

        public FuncionDebitoService(IUnitOfWork unit,
            IMapper mapper,
            IValidator<FuncionRequestDTO> validator,
            IValidator<FuncionRequestAsientoDTO> validatorAsiento)
        {
            _unit = unit;
            _mapper = mapper;
            _validator = validator;
            _validatorAsiento = validatorAsiento;
        }

        public async Task<Result<MovimientoDTO>> Contrasentar(FuncionRequestDTO funcion)
        {
            // Validaciones
            var validationResult = await _validator.ValidateAsync(funcion);

            if (!validationResult.IsValid)
            {
                List<string> errors = ValidationErrors.getValidationErrors(validationResult);
                return Result<MovimientoDTO>.Failure(errors);
            }

            if (funcion.Contrasiento != "C")
            {
                return Result<MovimientoDTO>.Failure("Error. Contrasiento de debito debe ser cadena 'C'");
            }

            // Busqueda de la cuenta
            var filterCuenta = new CuentaFilterDTO { NumeroCuenta = funcion.NumeroCuenta };
            var busquedaCuenta = await _unit._cuentaRepository.GetAllFilterOrderPage(filterCuenta, "", 1, 1);
            if (busquedaCuenta == null)
            {
                return Result<MovimientoDTO>.Failure("Error: No se pudo identificar la cuenta");
            }

            var cuenta = busquedaCuenta.FirstOrDefault();

            if (cuenta == null)
            {
                return Result<MovimientoDTO>.Failure("Error: No se pudo identificar la cuenta");
            }

            // Recuperacion del movimiento original
            MovimientoFilterDTO filterMovimiento = new MovimientoFilterDTO
            {
                FechaMovimiento = funcion.FechaMovimiento,
                NumeroComprobante = funcion.NumeroComprobante,
                NumeroCuenta = funcion.NumeroCuenta,
                Importe = funcion.Importe,
                Contrasiento = ""
            };

            // Busqueda de movimiento original
            var busquedaOriginal = await _unit._movimientoRepository.GetAllMovimientoFilterOrderPage(filterMovimiento, "", 1, 1);
            if (busquedaOriginal.Count() == 0)
            {
                return Result<MovimientoDTO>.Failure("Error: No se pudo encontrar el movimiento original");
            }
            var original = busquedaOriginal.FirstOrDefault();
            if (original == null)
            {
                return Result<MovimientoDTO>.Failure("Error: No se pudo encontrar el movimiento original");
            }

            try
            {
                return await _unit.ExecuteAsync(async () =>
                {

                    // Actualizacion del saldo
                    var saldoAnterior = cuenta.Capital;
                    cuenta.Capital += funcion.Importe;

                    // Actualizacion de la cuenta
                    var actualizacionCuenta = await _unit._cuentaRepository.UpdateItem(cuenta);

                    if (!actualizacionCuenta._success)
                    {
                        await _unit.RollbackAsync();
                        return Result<MovimientoDTO>.Failure($"Error en la actualizacion del saldo de la cuenta: {actualizacionCuenta._errorMessage}");
                    }

                    // Actualizacion de original y generacion del contrasiento
                    original.Contrasiento = "C";
                    var actualizacionMovimiento = await _unit._movimientoRepository.UpdateItem(original);
                    if (!actualizacionMovimiento._success)
                    {
                        await _unit.RollbackAsync();
                        return Result<MovimientoDTO>.Failure("Error en actualizacion de movimiento original");
                    }

                    Movimiento nuevoMovimiento = new Movimiento
                    {
                        IdMovimiento = 0,
                        IdCuenta = cuenta.IdCuenta,
                        Contrasiento = "M",
                        Importe = funcion.Importe,
                        FechaMovimiento = funcion.FechaMovimiento,
                        FechaReal = DateTime.Now,
                        NumeroComprobante = funcion.NumeroComprobante,
                        CodigoMovimiento = 0 /*Debito*/,
                        SaldoAnterior = saldoAnterior,
                        Descripcion = "Contrasiento Debito"
                    };

                    var registroMovimiento = await _unit._movimientoRepository.AddItem(nuevoMovimiento);
                    if (!registroMovimiento._success)
                    {
                        await _unit.RollbackAsync();
                        return Result<MovimientoDTO>.Failure($"Error en el registro del nuevo movimiento: {registroMovimiento._errorMessage}");
                    }

                    // Mapeo de retorno
                    var retorno = _mapper.Map<MovimientoDTO>(registroMovimiento._value);

                    return Result<MovimientoDTO>.Success(retorno);
                });
            }
            catch (Exception ex)
            {
                return Result<MovimientoDTO>.Failure($"Error en la operación de Contrasiento Débito: {ex.Message}");
            };
        }

        public async Task<Result<MovimientoDTO>> Ejecutar(FuncionRequestAsientoDTO funcion)
        {
            // Validaciones
            var validationResult = await _validatorAsiento.ValidateAsync(funcion);

            if (!validationResult.IsValid)
            {
                List<string> errors = ValidationErrors.getValidationErrors(validationResult);
                return Result<MovimientoDTO>.Failure(errors);
            }

            // Busqueda de la cuenta
            var filterCuenta = new CuentaFilterDTO { NumeroCuenta = funcion.NumeroCuenta };
            var busquedaCuenta = await _unit._cuentaRepository.GetAllFilterOrderPage(filterCuenta, "", 1, 1);
            if (busquedaCuenta == null)
            {
                return Result<MovimientoDTO>.Failure("Error: No se pudo identificar la cuenta");
            }

            var cuenta = busquedaCuenta.FirstOrDefault();

            if (cuenta == null)
            {
                return Result<MovimientoDTO>.Failure("Error: No se pudo identificar la cuenta");
            }

            // Control de saldo
            if (cuenta.Capital < funcion.Importe)
            {
                return Result<MovimientoDTO>.Failure("Error: Saldo insuficiente para realizar la operación");
            }

            // Recuperacion del proximo comprobante
            var busquedaComprobante = await _unit._numeracionRepository.GetByDescripcion("COMPROBANTE");

            if (busquedaComprobante == null)
            {
                await _unit.RollbackAsync();
                return Result<MovimientoDTO>.Failure("Error: No se pudo obtener el nro de comprobante");
            }
            try
            {
                return await _unit.ExecuteAsync(async () =>
                {
                    // Actualizacion del saldo
                    var saldoAnterior = cuenta.Capital;
                    cuenta.Capital -= funcion.Importe;

                    // Actualizacion de la cuenta
                    var actualizacionCuenta = await _unit._cuentaRepository.UpdateItem(cuenta);

                    if (!actualizacionCuenta._success)
                    {
                        await _unit.RollbackAsync();
                        return Result<MovimientoDTO>.Failure($"Error en la actualizacion del saldo de la cuenta: {actualizacionCuenta._errorMessage}");
                    }

                    // Generacion del asiento de debito
                    Movimiento nuevoMovimiento = new Movimiento
                    {
                        IdMovimiento = 0,
                        IdCuenta = cuenta.IdCuenta,
                        Contrasiento = "",
                        Importe = funcion.Importe,
                        FechaMovimiento = funcion.FechaMovimiento,
                        FechaReal = DateTime.Now,
                        NumeroComprobante = busquedaComprobante._value.UltimoNumero,
                        CodigoMovimiento = 0 /*Debito*/,
                        SaldoAnterior = saldoAnterior,
                        Descripcion = funcion.Descripcion
                    };

                    var registroMovimiento = await _unit._movimientoRepository.AddItem(nuevoMovimiento);
                    if (!registroMovimiento._success)
                    {
                        await _unit.RollbackAsync();
                        return Result<MovimientoDTO>.Failure($"Error en el registro del nuevo movimiento: {registroMovimiento._errorMessage}");
                    }

                    // Mapeo de retorno
                    var retorno = _mapper.Map<MovimientoDTO>(registroMovimiento._value);

                    return Result<MovimientoDTO>.Success(retorno);
                });
            }
            catch (Exception ex)
            {
                return Result<MovimientoDTO>.Failure($"Error en la operación Débito: {ex.Message}");
            };

        }
    }
}
