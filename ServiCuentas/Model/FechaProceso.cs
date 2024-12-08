using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Model
{
    public class FechaProceso
    {
        [Key]
        public int IdFechaProceso { get; set; }
        [MaxLength(20)]
        public string? Descripcion { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public DateTime FechaAnterior { get; set; }
        [Required]
        public DateTime FechaSiguiente { get; set; }
        [Required]
        public int Estado { get; set; } = 0;
    }
}
