using Microsoft.EntityFrameworkCore;

namespace lippi_homework.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base(new DbContextOptions<AppDbContext>()){}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Producto> Producto { get; set; }
    }
}
