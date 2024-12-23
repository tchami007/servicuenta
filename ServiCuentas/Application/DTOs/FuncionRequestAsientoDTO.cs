using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Application.DTOs
{
    public class FuncionRequestAsientoDTO
    {
        [Required]
        public decimal NumeroCuenta { get; set; }
        [Required]
        public DateTime FechaMovimiento { get; set; }
        [Required]
        public int CodigoMovimiento { get; set; }
        [Required]
        public Decimal Importe { get; set; }
        public string? Descripcion { get; set; }
    }
}
