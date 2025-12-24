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
        public DbSet<Area> Areas { get; set; }

        // --- HELPDESK ---
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketDetalle> TicketDetalles { get; set; } // Agregado para que compile Negocio
        public DbSet<EstadoTicket> Estados { get; set; } // Renombrado de 'EstadosTickets' a 'Estados'
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }

        // --- INVENTARIO ---
        public DbSet<Activo> Activos { get; set; }
        public DbSet<TipoActivo> TiposActivo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones específicas de Oracle si son necesarias
            modelBuilder.Entity<Usuario>()
                .Property(u => u.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}