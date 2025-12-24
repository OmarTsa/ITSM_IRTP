using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        // --- SEGURIDAD ---
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Area> Areas { get; set; } // <--- AGREGAR ESTA LÍNEA

        // --- HELPDESK ---
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<EstadoTicket> EstadosTickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }

        // --- INVENTARIO ---
        public DbSet<Activo> Activos { get; set; }
        public DbSet<TipoActivo> TiposActivo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional si fuera necesaria
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany()
                .HasForeignKey(u => u.IdRol);
        }
    }
}