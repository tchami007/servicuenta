namespace ServiCuentas.Application.DTOs
{
    public class CuentaFilterDTO
    {
        /// <summary>
        /// Descripcion de la cuenta
        /// </summary>
        public string? Descripcion { get; set; }
        /// <summary>
        /// Decimal que indica el numero de la cuenta
        /// </summary>
        public Decimal? NumeroCuenta { get; set; }
        /// <summary>
        /// Datetime que indica la fecha de alta de la cuenta
        /// </summary>
        public DateTime? FechaAlta { get; set; }
        /// <summary>
        /// int que indica el estado de la cuenta 0 activa 1 inactiva
        /// </summary>
        public int? Estado {  get; set; } 
    }
}
