using AppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Data;

public class DataContextEF:DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Users> Users { get; set; }
    public DbSet<UserSalary> UserSalaries { get; set; }
    public DbSet<UserJobInfo> UserJobInfos { get; set; }

    public DataContextEF(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"),
                optionsBuilder=>optionsBuilder.EnableRetryOnFailure());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SCOTT");

        modelBuilder.Entity<Users>().ToTable("users").HasKey(u=>u.UserId);
        modelBuilder.Entity<UserSalary>().ToTable("UserSalary").HasKey(u => u.UserId);
        modelBuilder.Entity<UserJobInfo>().ToTable("UserJobInfo").HasKey(u => u.UserId);
    }
}
