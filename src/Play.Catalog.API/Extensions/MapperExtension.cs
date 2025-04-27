using Play.Catalog.API.DTOs;
using Play.Catalog.API.Entites;

namespace Play.Catalog.API.Extensions;

public static class MapperExtensions
{
    public static ItemDTO AsDTO(this Item item)
    {
        return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
}