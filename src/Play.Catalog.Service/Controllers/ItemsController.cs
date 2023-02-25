using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDTO> _items = new()
    {
         new ItemDTO(Guid.NewGuid(), "Potion", "Restores small amount of HP", 5, DateTimeOffset.UtcNow),
         new ItemDTO(Guid.NewGuid(), "Antidote", "Cures posion", 7, DateTimeOffset.UtcNow),
         new ItemDTO(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
    };
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(ILogger<ItemsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<ItemDTO> Get() => _items;

    [HttpGet("{id}")]
    public ItemDTO GetById(Guid id) => _items.SingleOrDefault(x => x.Id == id);
}
