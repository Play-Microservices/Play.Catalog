using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entites;

namespace Play.Catalog.Service.Extensions;

public static class MapperExtensions
{
    public static ItemDTO AsDTO(this Item item)
    {
        return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
}