using Play.Common.Entites;

namespace Play.Catalog.API.Entites;

public class Item : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}