namespace Database;
using Models;

public class DatabaseContext : DbContextBase
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DemoModel> DemoModels { get; set; } = null!;
}