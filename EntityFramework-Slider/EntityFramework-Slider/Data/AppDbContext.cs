using EntityFramework_Slider.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SliderInfo> SliderInfos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<About> Abouts { get; set; }

        public DbSet<Adventage> Adventages { get; set; }

        public DbSet<Experts> Experts { get; set; }

        public DbSet<ExpertsHeader> ExpertsHeaders { get; set; }

        public DbSet<Subscribe> Subscribs { get; set; }

        public DbSet<BlogHeader> BlogHeaders { get; set; }

        public DbSet<Say> Says { get; set; }

        public DbSet<Instagram> Instagrams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(m => !m.SoftDelete);
        }

    }
}
