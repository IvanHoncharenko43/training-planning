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
            optionsBuilder.UseMySql("server=localhost;userid=root;port=3306;database=myDb",
                new MySqlServerVersion(new Version(8, 0, 41)));
        }
    }
}