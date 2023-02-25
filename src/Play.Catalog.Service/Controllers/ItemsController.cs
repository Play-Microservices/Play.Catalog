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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetById(Guid id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public ActionResult<ItemDTO> Post(CreateItemDTO createItem)
    {
        var item = new ItemDTO(Guid.NewGuid(), createItem.Name, createItem.Description, createItem.Price, DateTimeOffset.UtcNow);
        _items.Add(item);

        return CreatedAtAction(nameof(GetById), new { Id = item.Id }, item);
    }
}
