using Microsoft.EntityFrameworkCore;
using WpfApp1.Model;

namespace WpfApp1.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;database=myDb;password=myserver1234",
                new MySqlServerVersion(new Version(8, 0, 41)));
        }
    }
}