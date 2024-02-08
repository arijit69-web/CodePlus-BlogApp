using CodePlus.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePlus.API.Data
{
    /*
     Maintaining Connection To Db
     Track Changes
     Perform CRUD operations
     Bridge between .net models and the database
     */
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Dbset class represnts the collections of entities i.e. blog post and category
        // Entity Framework core ORM: a way to interact wiith the entities in the DB
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

    }
}
