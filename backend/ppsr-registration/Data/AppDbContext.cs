using Microsoft.EntityFrameworkCore;
using PpsrRegistration.Models;

namespace PpsrRegistration.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<VehicleRegistration> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleRegistration>()
            .HasIndex(v => new { v.GrantorFirstName, v.GrantorLastName });
    }
}
