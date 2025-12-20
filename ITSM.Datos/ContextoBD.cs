using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Activo> Activos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<EstadoTicket> EstadosTicket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Esquema por defecto (Tu usuario de Oracle)
            // Si el script lo hiciste portable (sin prefijo), esto ayuda a encontrar las tablas.
            modelBuilder.HasDefaultSchema("OTITO");

            // --- MAPEO DE USUARIOS (CRÍTICO) ---
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("SEG_USUARIOS"); // Nombre REAL de la tabla en Oracle

                entity.HasKey(e => e.IdUsuario);

                // Mapeo Columna Oracle <-> Propiedad C#
                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
                entity.Property(e => e.Username).HasColumnName("USERNAME");
                entity.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH");
                entity.Property(e => e.Nombres).HasColumnName("NOMBRES");
                entity.Property(e => e.Apellidos).HasColumnName("APELLIDOS");
                entity.Property(e => e.Correo).HasColumnName("CORREO");
                entity.Property(e => e.IdRol).HasColumnName("ID_ROL");
                entity.Property(e => e.Estado).HasColumnName("ESTADO");
            });

            // --- MAPEO DE OTRAS TABLAS (Ajustado a tu Script SQL) ---
            modelBuilder.Entity<Activo>().ToTable("ACT_INVENTARIO"); // Antes era ACTIVOS
            modelBuilder.Entity<Ticket>().ToTable("HD_TICKETS");     // Antes era TICKETS
            modelBuilder.Entity<Categoria>().ToTable("CATEGORIAS");
            modelBuilder.Entity<Prioridad>().ToTable("PRIORIDADES");
            modelBuilder.Entity<EstadoTicket>().ToTable("ESTADOS_TICKET");

            // Datos semilla (opcional)
            modelBuilder.Entity<Prioridad>().HasData(
                new Prioridad { IdPrioridad = 1, Nombre = "Alta", HorasSLA = 4 },
                new Prioridad { IdPrioridad = 2, Nombre = "Media", HorasSLA = 24 }
            );
        }
    }
}