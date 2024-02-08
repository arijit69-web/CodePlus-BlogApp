using CodePlus.API.Models.Domain;

namespace CodePlus.API.Repositories.Interface
{

/*
Repository Design pattern to separate the Data Access Layer from the Application
Provides interface without exposing implementation
Helps create abstraction: By doing that, the controller now has no awareness of what's being called through the Dbcontext, whether it's a SQL Server database or a MongoDB database, it has no idea about it.

By using repository pattern in ASP.Net Core, we developers can achieve several benefits:

That is decoupling the data access layer from the rest of the application, which makes it easier to maintain and test the application.
Providing a standard interface for accessing data which improves the consistency and readability of the code.
Now every connection to the database goes through the repository.
We can also improve the performance of the application by using caching, batching or other optimization techniques supporting multiple data sources, which allows the application to switch between different data sources without affecting the application logic.
*/
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid id); // '?' is for: If we able to find a category for the given id then we will return a category or else it will return null value

        Task<Category?> UpdateAsync(Category category);

        Task<Category?> DeleteAsync(Guid id);
    }
}
