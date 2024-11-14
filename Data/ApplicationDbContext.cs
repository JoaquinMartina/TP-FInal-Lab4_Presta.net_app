using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presta.net_app.Models;

namespace Presta.net_app.Data
{
    //Clase que se encargara del mapping de modelos a la BDD
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Prestatario> Prestatarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Estado> Estados { get; set; }
    }
}
