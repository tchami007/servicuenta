using ServiCuentas.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Application.DTOs
{
    public class MovimientoFilterDTO
    {
        [Required]
        public DateTime FechaMovimiento { get; set; }
        public Decimal? NumeroCuenta { get; set; }
        public Decimal? NumeroComprobante { get; set; }
        public string? Contrasiento { get; set; }
        public int? CodigoMovimiento { get; set; } = 0;
        public Decimal? Importe { get; set; } = 0;
    }
}
