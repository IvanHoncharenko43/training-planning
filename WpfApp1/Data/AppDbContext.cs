using Microsoft.EntityFrameworkCore;
using WpfApp1.Model;

namespace WpfApp1.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<TrainingNote> TrainingNotes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}