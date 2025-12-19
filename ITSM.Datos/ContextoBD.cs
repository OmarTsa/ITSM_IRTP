// ... imports
using ITSM.Entidades;
using Microsoft.EntityFrameworkCore;
using System;

public class ContextoBD : DbContext
{
    // ... constructor

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Activo> Activos { get; set; }

    // NUEVOS
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Prioridad> Prioridades { get; set; }
    public DbSet<EstadoTicket> EstadosTicket { get; set; }

    // ... OnModelCreating
}