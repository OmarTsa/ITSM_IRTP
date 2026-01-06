using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== PRUEBA DE CONEXIÓN A BASE DE DATOS ===\n");
        
        try
        {
            // Obtener cadena de conexión desde configuración
            var connectionString = "REEMPLAZAR_CON_CADENA_CONEXION";
            
            var optionsBuilder = new DbContextOptionsBuilder<ContextoBD>();
            optionsBuilder.UseOracle(connectionString);
            
            using (var context = new ContextoBD(optionsBuilder.Options))
            {
                Console.WriteLine("1. Probando conexión...");
                context.Database.CanConnect();
                Console.WriteLine("   ✓ Conexión exitosa\n");
                
                Console.WriteLine("2. Verificando tablas:");
                
                // Verificar Usuarios
                var usuariosCount = context.Usuarios.Count();
                Console.WriteLine($"   ✓ SEG_USUARIOS: {usuariosCount} registros");
                
                // Verificar Roles
                var rolesCount = context.Roles.Count();
                Console.WriteLine($"   ✓ SEG_ROLES: {rolesCount} registros");
                
                // Verificar Areas
                var areasCount = context.Areas.Count();
                Console.WriteLine($"   ✓ SEG_AREAS: {areasCount} registros");
                
                // Verificar Tickets
                var ticketsCount = context.Tickets.Count();
                Console.WriteLine($"   ✓ HD_TICKETS: {ticketsCount} registros");
                
                // Verificar Estados
                var estadosCount = context.Estados.Count();
                Console.WriteLine($"   ✓ ESTADOS_TICKET: {estadosCount} registros");
                
                // Verificar Categorías
                var categoriasCount = context.Categorias.Count();
                Console.WriteLine($"   ✓ CATEGORIAS: {categoriasCount} registros");
                
                // Verificar Prioridades
                var prioridadesCount = context.Prioridades.Count();
                Console.WriteLine($"   ✓ PRIORIDADES: {prioridadesCount} registros");
                
                // Verificar Activos
                var activosCount = context.Activos.Count();
                Console.WriteLine($"   ✓ ACT_INVENTARIO: {activosCount} registros");
                
                // Verificar Tipos de Activo
                var tiposCount = context.TiposActivo.Count();
                Console.WriteLine($"   ✓ ACT_TIPOS_ACTIVO: {tiposCount} registros");
                
                Console.WriteLine("\n3. Probando consulta con JOIN:");
                var ticketsConDatos = context.Tickets
                    .Include(t => t.Estado)
                    .Include(t => t.Categoria)
                    .Include(t => t.Prioridad)
                    .Take(5)
                    .ToList();
                    
                Console.WriteLine($"   ✓ Consultados {ticketsConDatos.Count} tickets con datos relacionados");
                
                Console.WriteLine("\n=== TODAS LAS PRUEBAS EXITOSAS ===");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ ERROR: {ex.Message}");
            Console.WriteLine($"\nDetalles: {ex.InnerException?.Message}");
            Console.WriteLine($"\nStack: {ex.StackTrace}");
        }
        
        Console.WriteLine("\nPresione cualquier tecla para salir...");
        Console.ReadKey();
    }
}
