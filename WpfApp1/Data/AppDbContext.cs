using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WpfApp1.Model;

namespace WpfApp1.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite("Data Source=app.db")
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}