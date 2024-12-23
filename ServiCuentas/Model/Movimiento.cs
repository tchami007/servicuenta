using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiCuentas.Model
{
    public class Movimiento
    {
        [Key]
        public int IdMovimiento { get; set; }
        [Required]
        public DateTime FechaReal {  get; set; }
        [Required]
        public DateTime FechaMovimiento { get; set; }
        [Required]
        public int IdCuenta { get; set; }
        [Required]
        public Decimal NumeroComprobante { get; set; }
        [MaxLength(1)]
        public string? Contrasiento { get; set; }
        [Required]
        public int CodigoMovimiento { get; set; } = 0;
        [Required]
        public Decimal Importe { get; set; } = 0;
        public Decimal BaseImponible { get; set; } = 0;
        public Decimal Alicuota { get; set; } = 0;
        public Decimal SaldoAnterior { get; set; } = 0;
        public string Descripcion { get; set; }

        // Relacion
        [ForeignKey ("IdCuenta")]
        public virtual Cuenta? Cuenta { get; set; }
    }
}
