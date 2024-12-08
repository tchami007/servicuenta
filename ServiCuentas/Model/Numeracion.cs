using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Model
{
    public class Numeracion
    {
        [Key]
        public int IdNumeracion { get; set; }
        [MaxLength(50)]
        public string? Descripcion { get; set; }
        [Required]
        public Decimal UltimoNumero { get; set; } = 0;
        [Required]
        public int Estado { get; set; } = 0;
    }
}
