using Microsoft.EntityFrameworkCore;
using SecondProject.Model;

namespace SecondProject.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
    }
   
}
