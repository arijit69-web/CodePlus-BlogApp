namespace CodePlus.API.Models.DTO
{
    /*
     Used to transfer data between different layers
     Typically contain a subset of the properties in the domain model
     For example transferring data over a network

     The advantage of using DTOs, is that they can help hiding implementation details of domain objects (aka. entities). Exposing entities through endpoints can become a security issue if we do not carefully handle what properties can be changed through what operations.
     Separation of Concerns ensures that the domain/business objects are not tightly coupled with the API or the view layer.
     High Performance: This is because they allow us to retrieve only the data that is needed.
     */
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }
    }
}
