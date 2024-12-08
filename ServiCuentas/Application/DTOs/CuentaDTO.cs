﻿using System.ComponentModel.DataAnnotations;

namespace ServiCuentas.Application.DTOs
{
    public class CuentaDTO
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
        [Required]
        public int Estado { get; set; } = 0;
    }
}