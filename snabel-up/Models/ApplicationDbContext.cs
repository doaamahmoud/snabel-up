using Microsoft.EntityFrameworkCore;

namespace snabel_up.Models
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Category> categories { get; set; }
        public DbSet<SupCategory> supCategories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Article> articles { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Branch> branches { get; set; }
        public DbSet<NewsLetter> newsLetters { get; set; }
        public DbSet<Login> Logins { get; set; }





    }
}
