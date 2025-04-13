using Microsoft.EntityFrameworkCore;
using PpsrRegistration.Models;

namespace PpsrRegistration.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<VehicleRegistration> Registrations => Set<VehicleRegistration>();
}
