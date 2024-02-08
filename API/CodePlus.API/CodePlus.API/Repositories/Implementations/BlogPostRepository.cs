using CodePlus.API.Data;
using CodePlus.API.Models.Domain;
using CodePlus.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePlus.API.Repositories.Implementations
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            // NOTE: Relationships to include multiple levels of related data using the .ThenInclude() method
            return await dbContext.BlogPosts.Include(x=>x.Categories).ToListAsync(); // In .NET Core, the Include method loads related data along with the main entity data in a single database query. It's used to specify related entities that should be included in the query. | The Include method is used for eager loading, which means related entities come in a single query and database round trips are reduced.


        }
        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
            {
                return null;
            }

            // Update BlogPost
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
            /*
              
               Entry(existingCategory): This method is used to get an EntityEntry object for the given entity (existingCategory). An EntityEntry represents an entity being tracked by the DbContext and provides various methods and properties to access information about the entity.
               CurrentValues: This property of EntityEntry gets the current property values of the entity. In other words, it represents the current state of the entity's properties.
               SetValues(category): This method sets the property values of the entity to the values of the specified object (category). This is a way to update an existing entity with the values from another entity.
             
             */

            // Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                dbContext.BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

    }
}
