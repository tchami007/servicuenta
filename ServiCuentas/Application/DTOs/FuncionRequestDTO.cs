using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Application.DTOs
{
    public class FuncionRequestDTO
    {
        [Required]
        public decimal NumeroCuenta { get; set; }
        [Required]
        public DateTime FechaMovimiento { get; set; }
        [Required]
        public Decimal NumeroComprobante { get; set; }
        [Required]  
        public int CodigoMovimiento { get; set; }
        public string Contrasiento { get; set; }
        [Required]
        public Decimal Importe { get; set; }

    }
}
