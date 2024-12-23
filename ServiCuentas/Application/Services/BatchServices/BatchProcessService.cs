using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services.FuncionServices;
using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

namespace ServiCuentas.Application.Services.BatchServices
{
    public class BatchProcessService : IBatchProcess<BatchProcessConfig>
    {

        private readonly ICuentaRepository _cuentas;
        private readonly IFechaProcesoRepository _fechas;
        private readonly IMovimientoRepository _movimientos;
        private readonly IFuncionDebito _debitos;

        public BatchProcessService(ICuentaRepository cuentas, IFechaProcesoRepository fechas, IMovimientoRepository movimientos)
        {
            _cuentas = cuentas;
            _fechas = fechas;
            _movimientos = movimientos;
        }

        public async Task<Result<string>> AperturaFechaProcess(BatchProcessConfig config)
        {
            var fecha = await _fechas.GetAll();
            var buscada = fecha.FirstOrDefault(x => x.Descripcion == "CUENTA");

            if (buscada == null)
            {
                return Result<string>.Failure("Error: No se pudo determinar la fecha de proceso");
            }

            if (buscada.FechaAnterior != buscada.Fecha)
            {
                return Result<string>.Failure("Error: El dia ya esta abierto");
            }

            buscada.Fecha = buscada.FechaSiguiente;
            buscada.FechaSiguiente = buscada.FechaSiguiente.AddDays(1);

            var resultado = await _fechas.UpdateItem(buscada);
            if (!resultado._success)
            {
                return Result<string>.Failure("Error: No se pudo actualizar la fecha de proceso");
            }

            return Result<string>.Success("AperturaFechaProcess: Proceso con Exito");

        }

        public async Task<Result<string>> CalculoProcess(BatchProcessConfig config)
        {
            Decimal? tasaNominalAnual = config.InteresTasaNominal;

            if (tasaNominalAnual == null || tasaNominalAnual == 0)
            {
                return Result<String>.Failure("Error: La tasa nominal es nula o cero, no se puede procesar calculo");
            }

            var tasaEfectiva = TasaConvert.Nominal3652Efectiva((decimal)tasaNominalAnual);

            var tasaDiaria = TasaConvert.Efectiva2DiariaExponencial(tasaEfectiva);

            var cuentas = await _cuentas.GetAll();

            foreach (var item in cuentas)
            {
                if (item.Estado == 0)
                {
                    var interes = item.Capital * tasaDiaria;
                    var resultado = item.Interes + interes;
                    item.Interes = resultado;

                    await _cuentas.UpdateItem(item);

                    interes = 0; resultado = 0;
                }
            }
            return Result<string>.Success("CalculoProcess: Proceso con Exito");
        }

        public async Task<Result<string>> CierreFechaProcess(BatchProcessConfig config)
        {
            var fecha = await _fechas.GetAll();
            var buscada = fecha.FirstOrDefault(x => x.Descripcion == "CUENTA");

            if (buscada== null)
            {
                return Result<string>.Failure("Error: No se pudo determinar la fecha de proceso");
            }

            if (buscada.FechaAnterior == buscada.Fecha)
            {
                return Result<string>.Failure("Error: El dia ya esta cerrado");
            }

            buscada.FechaAnterior = buscada.Fecha;
            var resultado = await _fechas.UpdateItem(buscada);
            if (!resultado._success)
            {
                return Result<string>.Failure("Error: No se pudo actualizar la fecha anterior");
            }

            return Result<string>.Success("CierreFechaProcess: Proceso con Exito");
        }

        public async Task<Result<string>> ComisionesProcess(BatchProcessConfig config)
        {
            string descripcion = "Cobro Comision";
            var fecha = await _fechas.GetAll();
            var buscada = fecha.FirstOrDefault(x => x.Descripcion == "CUENTA");

            if (buscada == null)
            {
                return Result<string>.Failure("Error: No se pudo determinar la fecha de proceso");
            }

            var importeComision = config.ComisionValor;

            if (importeComision == 0)
            {
                return Result<string>.Failure("Error: No se encuentra definido el importe de comision");
            }

            var cuentas = await _cuentas.GetAll();

            foreach (var item in cuentas)
            {
                if (item.Capital > importeComision)
                {
                    var resultado = await CobroComision(item.NumeroCuenta, buscada.Fecha, importeComision, descripcion);
                }
                else
                {
                    // no se pudo cobrar comision
                }
            }

            return Result<string>.Success("ComisionesProcess: Proceso con Exito");
        }
            public async Task<Result<string>> DevengamientoInteresProcess(BatchProcessConfig config)
        {
            var fecha = await _fechas.GetAll();
            var buscada = fecha.FirstOrDefault(x => x.Descripcion == "CUENTA");

            if (buscada== null)
            {
                return Result<string>.Failure("Error: No se pudo determinar la fecha de proceso");
            }

            var cuentas = await _cuentas.GetAll();

            foreach (var item in cuentas)
            {
                if (item.Estado == 0)
                {

                    var resultado = item.Interes + item.DevengadoAcumulado;

                    if (resultado > 0)
                    {
                        item.DevengadoAcumulado = item.DevengadoAcumulado + resultado;

                        await _cuentas.UpdateItem(item);

                        Movimiento nuevoMovimiento = new Movimiento
                        {
                            FechaReal = DateTime.Now,
                            FechaMovimiento = buscada.Fecha,
                            CodigoMovimiento = 3,
                            Descripcion = "Devengamiento",
                            NumeroComprobante = 0,
                            Contrasiento = "",
                            IdCuenta = item.IdCuenta,
                            Importe = resultado,
                            BaseImponible = 0,
                            Alicuota = 0,
                            SaldoAnterior = 0
                        };

                        await _movimientos.AddItem(nuevoMovimiento);

                        resultado = 0;
                    }
                }
            }
            return Result<string>.Success("DevengamientoInteresProcess: Proceso con Exito");
        }

        public Task<Result<string>> IngresoMasivoProcess(BatchProcessConfig config)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> LiquidacionInteresProcess(BatchProcessConfig config)
        {
            throw new NotImplementedException();
        }

        private async Task<Result<string>> CobroComision(Decimal numeroCuenta, DateTime fecha, Decimal importe, String descripcion)
        {
            FuncionRequestAsientoDTO requestDebito = new FuncionRequestAsientoDTO
            {
                NumeroCuenta = numeroCuenta,
                FechaMovimiento = fecha,
                Importe = importe,
                CodigoMovimiento = 0, /*Debito*/
                Descripcion = descripcion
            };

            var resultado = await _debitos.Ejecutar(requestDebito);

            if (!resultado._success)
            {
                return Result<string>.Failure($"Error: Cobro Comision: {resultado._errorMessage}");
            }

            return Result<string>.Success("Cobro Comision Exitoso");

        }
    }
}
