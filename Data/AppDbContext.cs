using Microsoft.EntityFrameworkCore;
using OdontoApi.Models;

namespace OdontoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Dentist> Dentists => Set<Dentist>();
        public DbSet<Procedure> Procedures => Set<Procedure>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
    }
}
