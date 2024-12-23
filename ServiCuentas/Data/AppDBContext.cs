using Microsoft.EntityFrameworkCore;
using ServiCuentas.Model;

namespace ServiCuentas.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base (options) 
        { 
        }

        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<FechaProceso> FechasProceso { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Numeracion> Numeraciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuenta>(entity =>
                {
                    entity.HasKey(e => e.IdCuenta);
                    entity.Property(e => e.FechaAlta).IsRequired();
                    entity.Property(e => e.NumeroCuenta).IsRequired().HasPrecision(12, 0).HasDefaultValue(0);
                    entity.Property(e => e.Capital).IsRequired().HasPrecision(12, 2).HasDefaultValue(0);
                    entity.Property(e => e.Interes).IsRequired().HasPrecision(12, 2).HasDefaultValue(0);
                    entity.Property(e => e.Ajuste).HasPrecision(12, 2).HasDefaultValue(0);
                    entity.Property(e => e.DevengadoAcumulado).HasPrecision(12, 2).HasDefaultValue(0);
                    entity.Property(e => e.Estado).IsRequired().HasDefaultValue(0);
                    entity.Property(e => e.Descripcion).HasMaxLength(50);
                    entity.Property(e => e.Cvu).HasMaxLength(22);
                    entity.Property(e => e.Alias).HasMaxLength(20);
                    entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                    entity.Property(e => e.Celular).IsRequired().HasMaxLength(20);
                }
            );
            modelBuilder.Entity<FechaProceso>(entity =>
            {
                entity.HasKey(e => e.IdFechaProceso);
                entity.Property(e => e.Descripcion).HasMaxLength(20);
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.FechaAnterior).IsRequired();
                entity.Property(e => e.FechaSiguiente).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasDefaultValue(0);
            }
            );
            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(e => e.IdMovimiento);
                entity.Property(e => e.FechaReal).IsRequired();
                entity.Property(e => e.FechaMovimiento).IsRequired();
                entity.Property(e => e.IdCuenta).IsRequired();
                entity.Property(e => e.NumeroComprobante).IsRequired().HasPrecision(12, 0);
                entity.Property(e => e.Contrasiento).IsRequired().HasMaxLength(1);
                entity.Property(e => e.CodigoMovimiento).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.Importe).IsRequired().HasPrecision(12,2).HasDefaultValue(0);
                entity.Property(e => e.BaseImponible).HasPrecision(12, 2).HasDefaultValue(0);
                entity.Property(e => e.Alicuota).HasPrecision(6, 2).HasDefaultValue(0);
                entity.Property(e => e.SaldoAnterior).HasPrecision(12, 2).HasDefaultValue(0);
                entity.Property(e => e.Descripcion).HasMaxLength(50);
            });
            modelBuilder.Entity<Numeracion>(entity =>
            {
                entity.HasKey(e => e.IdNumeracion);
                entity.Property(e=>e.Descripcion).IsRequired().HasMaxLength(50);
                entity.Property(e=>e.UltimoNumero).IsRequired().HasPrecision(12,0).HasDefaultValue(0);
                entity.Property(e=>e.Estado).IsRequired().HasDefaultValue(0);
            });

            // Relacion Movimientos con Cuentas

            modelBuilder.Entity<Movimiento>()
                .HasOne(c => c.Cuenta)
                .WithMany(c => c.Movimientos)
                .HasForeignKey(c => c.IdCuenta);


        }
    }
}
