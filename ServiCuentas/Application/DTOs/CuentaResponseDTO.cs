using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Application.DTOs
{
    public class CuentaResponseDTO
    {
        [Key]
        public int IdCuenta { get; set; }
        [Required]
        public Decimal NumeroCuenta { get; set; } = 0;
        [Required]
        public DateTime FechaAlta { get; set; }
        [MaxLength(50)]
        public string? Descripcion { get; set; }
        [Required]
        public decimal Capital { get; set; } = 0;
        [Required]
        public decimal Interes { get; set; } = 0;
        public decimal Ajuste { get; set; } = 0;
        public decimal DevengadoAcumulado { get; set; } = 0;
        [Required]
        public int Estado { get; set; } = 0;
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Celular { get; set; }
        [MaxLength(22)]
        public string? Cvu { get; set; }
        [MaxLength(20)]
        public string Alias { get; set; }

    }
}
