using AutoMapper;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Validators;
using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services
{
    public interface ICuentaService
    {
        Task<Result<CuentaDTO>> AddCuenta(CuentaDTO cuenta);
        Task<Result<CuentaDTO>> AddCuenta(string? descripcion);
        Task<Result<CuentaDTO>> DeleteCuenta(int id);
        Task<Result<CuentaDTO>> UpdateCuenta(int id,CuentaDTO cuenta);
        Task<Result<CuentaDTO>> GetCuentaById(int id);
        Task<Result<CuentaDTO>> GetCuentaByNumeroCuenta(decimal numero);
        Task<IEnumerable<CuentaDTO>> GetAllCuentas(CuentaFilterDTO filter, string order, int page, int pageSize);
    }

    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _repository;
        private readonly INumeracionRepository _numeracion;
        private readonly IMapper _mapper;
        private readonly CuentaValidator _validator;

        public CuentaService (ICuentaRepository repository, IMapper mapper, CuentaValidator validator, INumeracionRepository numeracion)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _numeracion = numeracion;
        }

        public async Task<Result<CuentaDTO>> AddCuenta(string descripcion)
        {
            // Inicio cuenta Nueva
            Cuenta cuentaNueva = new Cuenta();
            cuentaNueva.FechaAlta = DateTime.Today;
            cuentaNueva.Descripcion = descripcion;
            cuentaNueva.Capital = 0;
            cuentaNueva.Interes = 0;
            cuentaNueva.Ajuste = 0;
            cuentaNueva.Estado = 0;

            //Calculo Nro de Cuenta

            // Proximo Numero Cuenta
            string? concepto = "CUENTA";
            var consulta = await _numeracion.GetByDescripcion(concepto);
            if (!consulta._success)
            {
                return Result<CuentaDTO>.Failure(consulta._errorMessage);
            }

            // Asigno Ultimo Numero
            cuentaNueva.NumeroCuenta = consulta._value.UltimoNumero;

            // Calculo digito
            int digito = DigitoVerificador.Calcular(cuentaNueva.NumeroCuenta);
            cuentaNueva.NumeroCuenta = cuentaNueva.NumeroCuenta * 10 + digito;

            var retorno = await _repository.AddItem(cuentaNueva);

            if (!retorno._success)
            {
                return Result<CuentaDTO>.Failure($"Error en el registro: {retorno._errorMessage}");
            }

            // Mapeo y retorno
            var resultado = _mapper.Map<CuentaDTO>(retorno._value);

            return Result<CuentaDTO>.Success(resultado);
        }

        public async Task<Result<CuentaDTO>> AddCuenta(CuentaDTO cuenta)
        {
            // Validacion de parametros
            var validationResult = _validator.Validate(cuenta);

            if (!validationResult.IsValid)
            {
                List<string> errors = ValidationErrors.getValidationErrors(validationResult);
                return Result<CuentaDTO>.Failure(errors);
            }

            // Registro
            var cuentaNueva = _mapper.Map<Cuenta>(cuenta);

            // Calculo digito
            int digito = DigitoVerificador.Calcular(cuentaNueva.NumeroCuenta);
            cuentaNueva.NumeroCuenta = cuentaNueva.NumeroCuenta * 10 + digito;

            var retorno = await _repository.AddItem(cuentaNueva);

            if (!retorno._success)
            {
                return Result<CuentaDTO>.Failure($"Error en el registro: {retorno._errorMessage}");
            }

            // Mapeo y retorno
            var resultado = _mapper.Map<CuentaDTO>(retorno._value);

            return Result<CuentaDTO>.Success(resultado);
        }

        public async Task<Result<CuentaDTO>> DeleteCuenta(int id)
        {
            // Eliminacion
            var retorno = await _repository.DeleteItem(id);
            if (!retorno._success)
            {
                return Result<CuentaDTO>.Failure($"Error en la eliminacion: {retorno._errorMessage}");
            }
            
            var resultado = _mapper.Map<CuentaDTO>(retorno._value);

            return Result<CuentaDTO>.Success(resultado);
        }

        public async Task<IEnumerable<CuentaDTO>> GetAllCuentas(CuentaFilterDTO filter, string order, int page, int pageSize)
        {
            var resultado = await _repository.GetAllFilterOrderPage(filter, order, page, pageSize);

            return _mapper.Map<IEnumerable<CuentaDTO>>(resultado);
        }

        public async Task<Result<CuentaDTO>> GetCuentaById(int id)
        {
            var retorno = await _repository.GetById(id);
            if (!retorno._success)
            {
                return Result<CuentaDTO>.Failure($"Error en la recuperacion: {retorno._errorMessage}");
            }

            var resultado = _mapper.Map<CuentaDTO>(retorno._value);

            return Result<CuentaDTO>.Success(resultado);
        }

        public async Task<Result<CuentaDTO>> GetCuentaByNumeroCuenta(decimal numero)
        {
            CuentaFilterDTO filtro = new CuentaFilterDTO
            {
                NumeroCuenta = numero
            };
            var resultado = await _repository.GetAllFilterOrderPage(filtro, "", 1, 1);
            if (resultado.Count() != 1)
            {
                return Result<CuentaDTO>.Failure("Error Cuenta inexistente");
            }
            var cuenta = resultado.First();
            return Result<CuentaDTO>.Success(_mapper.Map<CuentaDTO>(cuenta));
        }

        public async Task<Result<CuentaDTO>> UpdateCuenta(int id, CuentaDTO cuenta)
        {
            // Control de identificadores
            if (id != cuenta.IdCuenta)
            {
                return Result<CuentaDTO>.Failure("Los identificadores de parametro y dto no se corresponden");
            }

            // Validacion de parametros
            var validationResult = _validator.Validate(cuenta);

            if (!validationResult.IsValid)
            {
                List<string> errors = ValidationErrors.getValidationErrors(validationResult);
                return Result<CuentaDTO>.Failure(errors);
            }

            // Modificacion
            var cuentaModificada = _mapper.Map<Cuenta>(cuenta);

            var retorno = await _repository.UpdateItem(cuentaModificada);
            if (!retorno._success)
            {
                return Result<CuentaDTO>.Failure($"Error en la modificacion: {retorno._errorMessage}");
            }
            var resultado = _mapper.Map<CuentaDTO>(retorno._value);

            return Result<CuentaDTO>.Success(resultado);

        }
    }
}
