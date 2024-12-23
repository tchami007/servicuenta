namespace ServiCuentas.Application.Services.BatchServices
{
    public class BatchProcessConfig
    {
        public string OperatoriaDescripcion {  get; set; }
        public bool IngresoMasivo {  get; set; }
        public bool ManejaInteres {  get; set; }
        public bool ManejaAjuste { get; set; }
        public bool CobraComision { get; set; }
        public decimal InteresTasaNominal {  get; set; }
        public string InteresDevengamientoPeriodicidad { get; set; }
        public string InteresLiquidacionPeriodicidad { get; set; }
        public int AjusteHistoria { get; set; }
        public string AjustePeriodicidad { get; set; }
        public decimal ComisionValor {get; set; }
        public string ComisionPeriodicidad { get; set; }   
    }
}
