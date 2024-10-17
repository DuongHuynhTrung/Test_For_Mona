using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasIndex(_ => _.EmployeeCode)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Position> Positions { get; set; }
}
