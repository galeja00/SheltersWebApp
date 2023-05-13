using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
namespace cv8_ASP.NET_v2.Models;


public class HarborContext : DbContext
{
    public HarborContext() {}
    
    public DbSet<Harbar> Harbars { get; set; }

    public DbSet<Dog> Dogs { get; set; }
    
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseLazyLoadingProxies().UseSqlite(@"Data Source=F:\UtulkyDB.db");
        options.LogTo(Console.WriteLine);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary Keys
        modelBuilder.Entity<Harbar>().HasKey(h => new { h.Id });
        modelBuilder.Entity<Dog>().HasKey(h => new { h.Id });
        modelBuilder.Entity<Address>().HasKey(a => new { a.Id });
        
        // Foreign Keys
        modelBuilder.Entity<Harbar>().HasMany(h => h.Dogs).WithOne(d =>d.Harbar).HasForeignKey(d => d.HarbarID);
        modelBuilder.Entity<Address>().HasMany(a => a.Harbers).WithOne(h => h.Address).HasForeignKey(h => h.AddressID);
    }
}