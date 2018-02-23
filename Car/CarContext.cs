using System.Data.Entity;

namespace Car
{
    public class CarContext:DbContext
    {
        public DbSet<Brand> Brands { get; set; }
    }
}