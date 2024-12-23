using System.ComponentModel.DataAnnotations;

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
        /// <summary>
        /// string que indica el numero de CVU/CBU
        /// </summary>
        public string? Cvu { get; set; }
        /// <summary>
        /// string que indica el alias de la cuenta
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// string que indica la direccion correo electronico
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// string que indica el numero de telefono
        /// </summary>
        public string? Celular { get; set; }

    }
}
