using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto_WEB.Models;

namespace Proyecto_WEB.Data
{
    // IdentityDbContext ya incluye las tablas de usuarios (login).
    // Aquí agregamos nuestras propias tablas.
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Cada DbSet representa una tabla de la base de datos.
        public DbSet<Producto> Productos { get; set; } = default!;
    }
}
