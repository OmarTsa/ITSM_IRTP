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
            // Esquema por defecto (comenta si no usas esquema OTITO)
            modelBuilder.HasDefaultSchema("OTITO");

            // --- 1. USUARIOS (Tabla SEG_USUARIOS) ---
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("SEG_USUARIOS");
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
                entity.Property(e => e.Username).HasColumnName("USERNAME");
                entity.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH");
                entity.Property(e => e.Nombres).HasColumnName("NOMBRES");
                entity.Property(e => e.Apellidos).HasColumnName("APELLIDOS");
                entity.Property(e => e.Correo).HasColumnName("CORREO");
                entity.Property(e => e.IdRol).HasColumnName("ID_ROL");
                entity.Property(e => e.Estado).HasColumnName("ESTADO");
            });

            // --- 2. TICKETS (Tabla HD_TICKETS) ---
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("HD_TICKETS");
            });

            // --- 3. ACTIVOS (Tabla ACT_INVENTARIO) ---
            modelBuilder.Entity<Activo>(entity =>
            {
                entity.ToTable("ACT_INVENTARIO");
            });

            // --- 4. TABLAS AUXILIARES (CORREGIDO AQUÍ) ---

            // CRÍTICO: Usar HD_CATEGORIAS en lugar de CATEGORIAS
            modelBuilder.Entity<Categoria>().ToTable("HD_CATEGORIAS");

            modelBuilder.Entity<Prioridad>().ToTable("PRIORIDADES");
            modelBuilder.Entity<EstadoTicket>().ToTable("ESTADOS_TICKET");

            // Datos semilla para asegurar funcionamiento de Prioridades
            modelBuilder.Entity<Prioridad>().HasData(
                new Prioridad { IdPrioridad = 1, Nombre = "Alta (Crítico)", HorasSLA = 4 },
                new Prioridad { IdPrioridad = 2, Nombre = "Media (Normal)", HorasSLA = 24 },
                new Prioridad { IdPrioridad = 3, Nombre = "Baja (Planificado)", HorasSLA = 72 }
            );
        }
    }
}